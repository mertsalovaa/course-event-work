"use client";

import { useEffect, useState } from "react";
import dynamic from "next/dynamic";
import ImportButton from "./import-button";
import Header from "./header";
import { ThemeKey } from "@/context/theme-context";
import {
  DashboardStatsDTO,
  EventDTO,
  ViewType,
} from "@/app/api/entities.types";
import Layout from "./layout";
import { useCategory } from "@/context/category-context";
import CategoryFilter from "./category-filter";
import StatisticsComponent from "./statistics";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { getStatistic } from "@/app/api/events";
import Link from "next/link";

const EventsMap = dynamic(() => import("./events-map"), {
  ssr: false,
});

interface Props {
  events: EventDTO[];
  // error: ApiError | null;
  // dashboard: DashboardStatsDTO;
}

export default function EventsTab({ events }: Props) {
  const [view, setView] = useState<ViewType>("map");
  const { category, setCategory } = useCategory();
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

  const filtered =
    events &&
    (category === "Всі"
      ? events
      : events.filter((e) => e.category === category));
  console.log(category);
  console.log(events);
  const scoreColor = (s: number) =>
    s > 70 ? "#e05555" : s > 45 ? "#f0a030" : "#3dd68c";

  useEffect(() => {
    let isMounted = true;

    const loadStats = async () => {
      try {
        // setLoading(true);

        const result = await getStatistic();

        if (result.data && isMounted) {
          setStats(result.data);
        }
      } finally {
        if (isMounted) {
          // setLoading(false);
        }
      }
    };

    loadStats();

    const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:7280/hubs/dashboard")
      .withAutomaticReconnect()
      .build();

    connection.on("dashboard:update", async (data: DashboardStatsDTO) => {
      // setLoading(true);

      setStats(data);

      setTimeout(() => {
        // setLoading(false);
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
    <Layout view={view} setView={setView}>
      {/* Filter chips */}

      {/* Content */}
      {view === "map" ? (
        <EventsMap events={filtered} />
      ) : (
        <div
          style={{
            padding: "0 24px 24px",
            paddingTop: "100px",
            backgroundColor: "var(--background)",
          }}
        >
          <StatisticsComponent
            showStatistic
            loading={false}
            error={false}
            stats={stats}
          />
          <CategoryFilter view={view} />

          {filtered.map((event, index) => (
            <div
              key={event.id}
              style={{
                display: "flex",
                alignItems: "center",
                gap: 14,
                padding: "12px 0",
                borderBottom: "0.5px solid rgba(169, 157, 157, 0.698)",
                cursor: "pointer",
              }}
            >
              <div
                style={{
                  width: 8,
                  height: 8,
                  borderRadius: "50%",
                  flexShrink: 0,
                  background: scoreColor(Number.parseInt(event.priorityScore)),
                }}
              />
              <div style={{ flex: 1, minWidth: 0 }}>
                <Link
                  href={`/events/${event.id}`}
                  style={{
                    fontSize: 13,
                    fontWeight: 500,
                    color: "var(--text)",
                    whiteSpace: "nowrap",
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                  }}
                >
                  {event.title}
                </Link>
                <div
                  className="flex gap-1"
                  // style={{
                  //   fontSize: 11,
                  //   color: "var(--muted-teal)",
                  //   marginTop: 2,
                  //   fontFamily: "monospace",
                  // }}
                >
                  <p
                    style={{
                      fontSize: 12,
                      color: "var(--muted-teal)",
                      // marginTop: 2,
                      fontFamily: "monospace",
                    }}
                  >
                    {event.city}
                    {event.city && ","} {event.country} ·
                  </p>{" "}
                  <p
                    style={{
                      fontSize: 12,
                      color: scoreColor(Number.parseInt(event.priorityScore)),
                      // marginTop: 2,
                      fontWeight: "550",
                      fontFamily: "monospace",
                    }}
                  >
                    {event.category}
                  </p>
                </div>
              </div>
              <div
                style={{
                  fontFamily: "monospace",
                  fontSize: 11,
                  padding: "3px 9px",
                  borderRadius: 6,
                  flexShrink: 0,
                  color: scoreColor(Number.parseInt(event.priorityScore)),
                  background:
                    scoreColor(Number.parseInt(event.priorityScore)) + "18",
                }}
              >
                {event.priorityScore}
              </div>
            </div>
          ))}
        </div>
      )}
    </Layout>
  );
}
