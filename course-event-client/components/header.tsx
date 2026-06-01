"use client";

import styled from "styled-components";
import ImportButton from "./import-button";
import { LogoSubText, LogoText } from "./typography";
import { MapIcon } from "@/images/icons/map-icon";
import { ListIcon } from "@/images/icons/list-icon";
import { Dispatch, SetStateAction, useEffect, useState } from "react";
import { getStatistic } from "@/app/api/events";
import { HubConnectionBuilder } from "@microsoft/signalr";
import ThemeSwitcher from "./theme-switch";
import { DashboardStatsDTO } from "@/app/api/entities.types";
import { CATEGORIES, useCategory } from "@/context/category-context";
import CategoryFilter from "./category-filter";
import StatisticsComponent from "./statistics";
import { HeaderBody } from "./header-block";

export default function Header({
  view,
  setView,
}: {
  view: "map" | "list";
  setView: Dispatch<SetStateAction<"map" | "list">>;
}) {
  const [stats, setStats] = useState<DashboardStatsDTO>({
    totalEvents: 0,
    todayEvents: 0,
    criticalEvents: 0,
    countriesCount: 0,
    averagePriorityScore: 0,
    sourcesCount: 0,
    mostPopularCategory: "",
    mostActiveCountry: "",
  });

  const [loading, setLoading] = useState(false);
  const [showStatistic, setShowStatistic] = useState(false);

  const [error, setError] = useState(false);

   useEffect(() => {
    let isMounted = true;

    const loadStats = async () => {
      try {
        setLoading(true);

        const result = await getStatistic();

        if (result.data && isMounted) {
          setStats(result.data);
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    loadStats();

    const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:7280/hubs/dashboard")
      .withAutomaticReconnect()
      .build();

    connection.on("dashboard:update", async (data: DashboardStatsDTO) => {
      setLoading(true);

      setStats(data);

      setTimeout(() => {
        setLoading(false);
      }, 500);
    });

    connection
      .start()
      .then(() => console.log("SignalR connected"))
      .catch(console.error);

    return () => {
      isMounted = false;
      connection.stop();
    };
  }, []);

  return (
    <HeaderStyle $isMapView={view === "map"}>
      <HeaderBody>
        <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
          <div
            style={{
              width: 12,
              height: 12,
              borderRadius: "50%",
              background: "var(--lime-cream)",
              // border: "0.5px solid #c9ef69",
              boxShadow: "var(--boxShadow) 0px 0.5px 2px 1px",
            }}
          />
          <div>
            <LogoText>EventScope</LogoText>
            <LogoSubText>LIVE FEED · EVENTS · STRUCTURE</LogoSubText>
          </div>
        </div>
        <div
          style={{
            display: "flex",
            borderRadius: 10,
            flexWrap: "wrap",
            justifyContent: "space-between",
            rowGap: "8px",
          }}
          className="gap-4 md:gap-4"
        >
          {" "}
          <div className="flex gap-2">
            {" "}
            {view === "map" && (
              <button
                onClick={() => setShowStatistic(!showStatistic)}
                className="hover:opacity-75 flex gap-2"
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: 7,
                  padding: "7px 16px",
                  borderRadius: 8,
                  cursor: "pointer",
                  background: "transparent",
                  border: "1px solid rgba(255,255,255,0.08)",
                  color: "var(--text)",
                  fontFamily: "inherit",
                  fontSize: 13,
                  fontWeight: 500,
                  transition: "all 0.18s",
                }}
              >
                {showStatistic ? "Сховати" : "Показати"} статистику
              </button>
            )}
            <ImportButton />
          </div>
          <div className="flex gap-2">
            {(["map", "list"] as const).map((t) => (
              <button
                key={t}
                onClick={() => setView(t)}
                style={{
                  padding: "8px 16px",
                  borderRadius: 8,
                  fontSize: 13,
                  fontWeight: 600,
                  cursor: "pointer",
                  border: `1px solid ${t === view ? "var(--bronze-spice)" : "var(--text)"}`,
                  background: "transparent",
                  color: t === view ? "var(--bronze-spice)" : "var(--text)",
                }}
                className="flex gap-2 items-center hover:opacity-75"
              >
                {" "}
                {t === "map" ? (
                  <>
                    {" "}
                    <MapIcon
                      color={t === view ? "var(--bronze-spice)" : "var(--text)"}
                    />{" "}
                    <p>Карта</p>{" "}
                  </>
                ) : (
                  <>
                    {" "}
                    <ListIcon
                      color={t === view ? "var(--bronze-spice)" : "var(--text)"}
                    />{" "}
                    <p>Список</p>{" "}
                  </>
                )}{" "}
              </button>
            ))}{" "}
          </div>{" "}
        </div>{" "}
      </HeaderBody>{" "}
      {view === "map" && (
        <div className="flex justify-between w-full items-start gap-4">
          <HeaderBody className="w-auto! gap-2! p-3! flex-col">
            {" "}
            <ThemeSwitcher />
            <CategoryFilter view={view} />
          </HeaderBody>
          {view === "map" && (
            <StatisticsComponent
              showStatistic={showStatistic}
              loading={loading}
              stats={stats}
              error={error}
            />
          )}
        </div>
      )}
    </HeaderStyle>
  );
}



const HeaderStyle = styled.header<{ $isMapView?: boolean }>`
  position: absolute;
  top: 8px;
  left: 8px;
  right: 8px;

  display: flex;
  flex-direction: column;
  align-items: flex-end;
  justify-content: space-between;

  /* border-bottom: 0.5px solid rgb(178, 148, 148); */
  background: transparent;

  z-index: 9980;
  border-radius: 16px;
  gap: 6px;
`;
