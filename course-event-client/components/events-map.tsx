"use client";

import { Dispatch, SetStateAction, useState } from "react";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import L from "leaflet";
import Link from "next/link";
import { ThemeKey, useTheme } from "@/context/theme-context";
import { EventDTO } from "@/app/api/entities.types";
import { MultiEventPopup, SingleEventPopup } from "./event-popups";


export const scoreColor = (s: number) =>
  s > 70 ? "#e05555" : s > 45 ? "#f0a030" : "#3dd68c";

const createCustomIcon = (score: number) => {
  const color = scoreColor(score);
  return L.divIcon({
    className: "",
    html: `
      <div style="position:relative;width:20px;height:20px">
        <div style="position:absolute;inset:0;border-radius:50%;background:${color};opacity:0.2;animation:ping 2s ease-out infinite;"></div>
        <div style="position:absolute;inset:4px;border-radius:50%;background:${color};box-shadow:0 0 8px ${color}88;"></div>
      </div>`,
    iconSize: [20, 20],
    iconAnchor: [10, 10],
    popupAnchor: [0, -14],
  });
};

interface Props {
  events: EventDTO[];
}

export default function EventsMap({ events }: Props) {
  // const theme = THEMES[themeKey];
  const { theme } = useTheme();

  const tileUrl = {
    dark: "https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png",
    light:
      "https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png",
  }[theme];

  const groupedEvents = events
    .filter(
      (e) =>
        e.latitude !== null &&
        e.longitude !== null &&
        Number.isFinite(Number(e.latitude)) &&
        Number.isFinite(Number(e.longitude)),
    )
    .reduce(
      (acc, event) => {
        const key = `${event.latitude},${event.longitude}`;
        if (!acc[key]) acc[key] = [];
        acc[key].push(event);
        return acc;
      },
      {} as Record<string, EventDTO[]>,
    );

  return (
    <div style={{ position: "relative", height: "100vh" }}>
      {/* Перемикач тем */}

      <MapContainer
        center={[48, 15]}
        zoom={4}
        minZoom={3} // ← мінімальний зум (не дасть зменшити до повторення)
        maxZoom={18}
        maxBounds={[
          [-90, -180],
          [90, 180],
        ]} // ← обмежує панорамування
        maxBoundsViscosity={1.0} // ← "пружина" — не дає вийти за межі
        style={{ height: "100%", width: "100%" }}
      >
        <TileLayer
          key={theme}
          url={tileUrl}
          attribution='© <a href="https://carto.com/">CARTO</a>'
          //   noWrap={true}
        />

        {Object.entries(groupedEvents).map(([coords, group]) => {
          const [lat, lng] = coords.split(",").map(Number);
          const maxScore = Math.max(
            ...group.map((e) => Number(e.priorityScore)),
          );

          return (
            <Marker
              key={coords}
              position={[lat, lng]}
              icon={createCustomIcon(maxScore)} // колір по найвищому score
            >
              <Popup key={theme} className={`popup-${theme}`} maxWidth={280}>
                <div style={{ minWidth: 240 }}>
                  {/* Якщо одна подія — звичайний попап */}
                  {group.length === 1 ? (
                    <SingleEventPopup event={group[0]} />
                  ) : (
                    /* Якщо кілька — список */
                    <MultiEventPopup events={group} />
                  )}
                </div>
              </Popup>
            </Marker>
          );
        })}
      </MapContainer>
    </div>
  );
}
