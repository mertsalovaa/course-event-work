import Link from "next/link";
import { getEventById } from "@/app/api/events";
import styled from "styled-components";
import Image from "next/image";
import { scoreColor } from "@/components/events-map";

interface Props {
  params: Promise<{ id: string }>;
}

export const scoreLabel = (s: number) =>
  s > 70
    ? "високий пріоритет"
    : s > 45
      ? "середній пріоритет"
      : "низький пріоритет";

const scoreBadgeBg = (s: number) =>
  s > 70 ? "#2a0d0d" : s > 45 ? "#2a1e08" : "#0d2218";

export default async function EventDetailPage({ params }: Props) {
  const { id } = await params;
  const { data, error } = await getEventById(id);

  return error ? (
    <MainBody>
      <span
        style={{
          fontFamily: "monospace",
          fontSize: 11,
          color: "var(--text)",
        }}
      >
        {error.type === "network"
          ? "Сервер недоступний. Перевір підключення."
          : error.status === 404
            ? "Дані не знайдено."
            : error.message}
      </span>
    </MainBody>
  ) : (
    data && (
      <MainBody>
        {/* Навігація */}
        <nav
          style={{
            display: "flex",
            alignItems: "center",
            gap: 10,
            padding: "16px 24px",
            borderBottom: "0.5px solid rgba(255,255,255,0.07)",
            background: "var(--background)",
          }}
        >
          <Link
            href="/"
            style={{
              display: "flex",
              alignItems: "center",
              gap: 6,
              fontSize: 12,
              color: "var(--text)",
              textDecoration: "none",
              border: "0.5px solid rgba(255,255,255,0.08)",
              padding: "5px 12px",
              borderRadius: 7,
              letterSpacing: "0.04em",
            }}
          >
            ← Назад
          </Link>
          <span
            style={{
              fontFamily: "monospace",
              fontSize: 11,
              color: "var(--text)",
            }}
          >
            / events / {data!.title}
          </span>
        </nav>

        {/* Hero */}
        <div
          style={{
            padding: "32px 24px 24px",
            borderBottom: "0.5px solid rgba(255,255,255,0.06)",
          }}
        >
          {/* Бейджі */}
          <div
            style={{
              display: "flex",
              gap: 8,
              marginBottom: 16,
              flexWrap: "wrap",
            }}
          >
            <span
              style={{
                fontFamily: "monospace",
                fontSize: 10,
                padding: "3px 10px",
                borderRadius: 5,
                background: "var(--background)",
                color: "var(--muted-teal)",
                border: "0.5px solid rgba(255,255,255,0.08)",
                letterSpacing: "0.08em",
                textTransform: "uppercase",
              }}
            >
              {data.category}
            </span>
            <span
              style={{
                fontFamily: "monospace",
                fontSize: 10,
                padding: "3px 10px",
                borderRadius: 5,
                background: scoreBadgeBg(Number.parseInt(data.priorityScore)),
                color: scoreColor(Number.parseInt(data.priorityScore)),
                letterSpacing: "0.08em",
                textTransform: "uppercase",
              }}
            >
              Score {Number.parseInt(data.priorityScore)}
            </span>
            <span
              style={{
                fontFamily: "monospace",
                fontSize: 10,
                padding: "3px 10px",
                borderRadius: 5,
                background: "var(--background)",
                color: "var(--muted-teal)",
                border: "0.5px solid rgba(255,255,255,0.08)",
                letterSpacing: "0.08em",
                textTransform: "uppercase",
              }}
            >
              {data.country}
            </span>
          </div>

          {/* Заголовок */}
          <h1
            style={{
              fontSize: 22,
              fontWeight: 700,
              lineHeight: 1.35,
              color: "var(--text)",
              marginBottom: 16,
              letterSpacing: "-0.01em",
            }}
          >
            {data.title}
          </h1>

          {/* Мета */}
          <div style={{ display: "flex", gap: 16, flexWrap: "wrap" }}>
            {[
              { icon: "📍", text: `${data.city}, ${data.region}` },
              { icon: "📅", text: data.date },
              {
                icon: "🕐",
                text: `опубліковано ${new Date(data.publishedAt).toLocaleDateString("uk-UA")}`,
              },
            ].map((m, i) => (
              <span
                key={i}
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: 5,
                  fontSize: 12,
                  color: "var(--text)",
                  fontFamily: "monospace",
                }}
              >
                {m.icon} {m.text}
              </span>
            ))}
          </div>
        </div>

        {/* Body */}
        <div style={{ display: "grid", gridTemplateColumns: "1fr 260px" }}>
          {/* Основний контент */}
          <div
            style={{
              padding: 24,
              borderRight: "0.5px solid rgba(255,255,255,0.06)",
            }}
          >
            {/* Score */}
            <div style={{ marginBottom: 28 }}>
              <div
                style={{
                  fontFamily: "monospace",
                  fontSize: 10,
                  color: "var(--bronze-spice)",
                  letterSpacing: "0.12em",
                  textTransform: "uppercase",
                  marginBottom: 10,
                }}
              >
                Priority Score
              </div>
              <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
                <span
                  style={{
                    fontFamily: "monospace",
                    fontSize: 32,
                    fontWeight: 500,
                    color: scoreColor(Number.parseInt(data.priorityScore)),
                  }}
                >
                  {data.priorityScore}
                </span>
                <div style={{ flex: 1 }}>
                  <div
                    style={{
                      height: 4,
                      background: "rgba(255,255,255,0.07)",
                      borderRadius: 2,
                      marginBottom: 5,
                    }}
                  >
                    <div
                      style={{
                        width: `${data.priorityScore}%`,
                        height: "100%",
                        background: scoreColor(Number.parseInt(data.priorityScore)),
                        borderRadius: 2,
                      }}
                    />
                  </div>
                  <div
                    style={{
                      fontSize: 11,
                      color: "var(--text)",
                      fontFamily: "monospace",
                    }}
                  >
                    {scoreLabel(Number.parseInt(data.priorityScore))} · {Number.parseInt(data.priorityScore)} / 100
                  </div>
                </div>
              </div>
            </div>

            {/* Опис */}
            <div style={{ marginBottom: 28 }}>
              <div
                style={{
                  fontFamily: "monospace",
                  fontSize: 10,
                  color: "var(--text)",
                  letterSpacing: "0.12em",
                  textTransform: "uppercase",
                  marginBottom: 10,
                }}
              >
                Опис
              </div>
              <p
                style={{
                  fontSize: 14,
                  lineHeight: 1.75,
                  color: "var(--muted-teal)",
                  margin: 0,
                }}
              >
                {data.description}
              </p>
            </div>

            {/* Джерела */}
            <div>
              <div
                style={{
                  fontFamily: "monospace",
                  fontSize: 10,
                  color: "var(--text)",
                  letterSpacing: "0.12em",
                  textTransform: "uppercase",
                  marginBottom: 10,
                }}
              >
                Джерела ({data.sources.length})
              </div>
              <div className="flex flex-wrap w-full gap-4а">
                {data.sources.map((source, i) => (
                  <a
                    key={i}
                    href={source.url}
                    target="_blank"
                    rel="noopener noreferrer"
                    style={{
                      display: "flex",
                      alignItems: "flex-start",
                      gap: 12,
                      padding: 12,
                      borderRadius: 9,
                      border: "0.5px solid rgba(255,255,255,0.06)",
                      background: "var(--text)",
                      marginBottom: 8,
                      textDecoration: "none",
                      cursor: "pointer",
                    }}
                  >
                    {/* Зображення або заглушка */}
                    <div
                      style={{
                        width: 40,
                        height: 40,
                        borderRadius: 6,
                        background: "var(--text)",
                        flexShrink: 0,
                        overflow: "hidden",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        color: "var(--background)",
                        fontSize: 18,
                      }}
                    >
                      {source.imageUrl ? (
                        <Image
                          src={source.imageUrl}
                          alt=""
                          style={{
                            width: "100%",
                            height: "100%",
                            objectFit: "cover",
                          }}
                        />
                      ) : (
                        "📰"
                      )}
                    </div>

                    <div style={{ flex: 1, minWidth: 0 }}>
                      <div
                        style={{
                          fontSize: 13,
                          fontWeight: 500,
                          color: "var(--muted-teal)",
                          marginBottom: 3,
                          whiteSpace: "nowrap",
                          overflow: "hidden",
                          textOverflow: "ellipsis",
                          gap: 6,
                        }}
                      >
                        {source.sourceName || new URL(source.url).hostname}
                      </div>
                      {source.authorName && (
                        <div
                          style={{
                            fontSize: 11,
                            color: "var(--background)",
                            fontFamily: "monospace",
                            marginBottom: 3,
                          }}
                        >
                          {source.authorName}
                        </div>
                      )}
                      {source.publishedAt && (
                        <div
                          style={{
                            fontSize: 10,
                            color: "var(--background)",
                            fontFamily: "monospace",
                          }}
                        >
                          {new Date(source.publishedAt).toLocaleDateString(
                            "uk-UA",
                          )}
                        </div>
                      )}
                    </div>

                    <span
                      style={{
                        color: "var(--background)",
                        fontSize: 16,
                        fontWeight: 500,
                        flexShrink: 0,
                        marginTop: 2,
                      }}
                    >
                      ↗
                    </span>
                  </a>
                ))}
              </div>
            </div>
          </div>

          {/* Сайдбар */}
          <div style={{ padding: "24px 20px" }}>
            <div
              style={{
                fontFamily: "monospace",
                fontSize: 10,
                color: "var(--text)",
                letterSpacing: "0.12em",
                textTransform: "uppercase",
                marginBottom: 10,
              }}
            >
              Деталі
            </div>
            {[
              { icon: "🌍", key: "Країна", val: data.country },
              { icon: "🗺", key: "Регіон", val: data.region },
              { icon: "🏙", key: "Місто", val: data.city },
              { icon: "🏷", key: "Категорія", val: data.category },
              {
                icon: "📍",
                key: "Координати",
                val: `${Number(data.latitude).toFixed(4)}, ${Number(data.longitude).toFixed(4)}`,
              },
            ].map((row, i) => (
              <div
                key={i}
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  padding: "8px 0",
                  borderBottom: "0.5px solid rgba(255,255,255,0.04)",
                }}
              >
                <span
                  style={{
                    fontSize: 11,
                    color: "var(--text)",
                    display: "flex",
                    alignItems: "center",
                    gap: 6,
                    opacity: 0.8,
                  }}
                >
                  {row.key}
                </span>
                <span
                  style={{
                    fontSize: 12,
                    color: "var(--text)",
                    fontFamily: "monospace",
                    textAlign: "right",
                    maxWidth: 130,
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                    whiteSpace: "nowrap",
                  }}
                >
                  {row.val}
                </span>
              </div>
            ))}
          </div>
        </div>
      </MainBody>
    )
  );
}

export const MainBody = styled.main<{$isMapView?: boolean}>`
  font-family: 'Syne', sans-serif;
  background: var(--background);
  color: var(--text);
  min-height: 100vh'
`;
