import { EventDTO } from "@/app/api/entities.types";
import Link from "next/link";
import { useState } from "react";
import { scoreColor } from "./events-map";
import { LocationIcon } from "@/images/icons/location-icon";
import { PinIcon } from "@/images/icons/pin-icon";
import { ChevronIcon } from "@/images/icons/chevron-icon";

export function MultiEventPopup({ events }: { events: EventDTO[] }) {
  const [selected, setSelected] = useState<number>(0);
  const event = events[selected];

  return (
    <div style={{ minWidth: 240 }}>
      {/* Лічильник і навігація */}
      <div
        style={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          marginBottom: 10,
          paddingBottom: 8,
          borderBottom: "0.5px solid rgba(255,255,255,0.07)",
        }}
      >
        <div className="flex gap-2 items-center">
          <PinIcon color="var(--popupText)" size={14} />
          <p
            className="m-0!"
            style={{ fontFamily: "monospace", fontSize: 12, color: "var(--popupText)" }}
          >
            {events.length} подій на локації
          </p>
        </div>
        <div style={{ display: "flex", alignItems: "center", gap: 4 }}>
          <button
            onClick={() => setSelected((i) => Math.max(0, i - 1))}
            disabled={selected === 0}
            style={{
              background: "none",
              border: "none",
              color: "var(--popupSubtext)",
              cursor: selected === 0 ? "not-allowed" : "pointer",
              fontSize: 14,
            }}
          >
            <ChevronIcon direction="left" size={14} color="var(--popupSubtext)" />
          </button>
          <span
            style={{ fontFamily: "monospace", fontSize: 11, color: "var(--popupSubtext)" }}
          >
            {selected + 1} / {events.length}
          </span>
          <button
            onClick={() =>
              setSelected((i) => Math.min(events.length - 1, i + 1))
            }
            disabled={selected === events.length - 1}
            style={{
              background: "none",
              border: "none",
              color: "var(--popupSubtext)",
              cursor:
                selected === events.length - 1 ? "not-allowed" : "pointer",
              fontSize: 14,
            }}
          >
            <ChevronIcon direction="right" size={14} color="var(--popupSubtext)" />
          </button>
        </div>
      </div>

      {/* Поточна подія */}
      <SingleEventPopup event={event} />
    </div>
  );
}

export function SingleEventPopup({ event }: { event: EventDTO }) {
  const score = Number(event.priorityScore);
  return (
    <div className="flex flex-col gap-1">
      <div
        style={{
          fontSize: 10,
          letterSpacing: "0.1em",
          textTransform: "uppercase",
          color: scoreColor(score),
          fontFamily: "monospace",
        }}
      >
        {event.category}
      </div>
      <div
        style={{
          fontSize: 13,
          fontWeight: 500,
          color: "var(--popupText)",
          lineHeight: 1.4,
        }}
      >
        {event.title}
      </div>
      <div className="flex gap-2 items-center">
        <LocationIcon color="var(--popupSubtext)" size={16} />
        <p
          className="my-2!"
          style={{ fontSize: 12, color: "var(--popupSubtext)" }}
        >
          {event.city}{event.city && ","} {event.country}
        </p>
      </div>
      <div
        className="py-3"
        style={{
          display: "flex",
          alignItems: "center",
          gap: 6,
          borderTop: "0.5px solid var(--boxShadow)",
        }}
      >
        <span
          style={{
            fontSize: 11,
            color: "var(--popupText)",
            fontFamily: "monospace",
          }}
        >
          Рейтинг
        </span>
        <div
          style={{
            flex: 1,
            height: 4,
            background: "transparent",
            border: "0.5px solid var(--popupShadow)",
            borderRadius: 2,
          }}
        >
          <div
            style={{
              width: `${score}%`,
              height: "100%",
              background: scoreColor(score),
              borderRadius: 2,
            }}
          />
        </div>
        <span
          style={{
            fontSize: 13,
            fontWeight: '600',
            fontFamily: "monospace",
            color: scoreColor(score),
          }}
        >
          {score}
        </span>
      </div>
      <Link
        href={`/events/${event.id}`}
        style={{
          display: "block",
          textAlign: "center",
          padding: "6px 0",
          background: "transparent",
          borderRadius: 6,
          color: "var(--popupText)",
          fontSize: 12,
          textDecoration: "none",
          border: "0.5px solid var(--boxShadow)",
        }}
      >
        Детальніше →
      </Link>
    </div>
  );
}
