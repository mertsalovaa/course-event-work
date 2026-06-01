import styled from "styled-components";
import { DashboardStatsDTO } from "@/app/api/entities.types";
import { HeaderBody } from "./header-block";

export default function StatisticsComponent({
  showStatistic,
  stats,
  loading,
  error,
}: {
  showStatistic: boolean;
  stats: DashboardStatsDTO;
  loading: boolean;
  error: boolean;
}) {
  return (
    showStatistic &&
    (loading ? (
      <HeaderBody className="w-auto! gap-4 py-3!">
        <h4>Завантаження статистики...</h4>
      </HeaderBody>
    ) : error ? (
      <HeaderBody className="w-auto! gap-4 py-3!">
        <h4
          style={{
            color: "#ff7b7b",
          }}
        >
          Сервер тимчасово недоступний
        </h4>
      </HeaderBody>
    ) : (
      <StatisticBlock>
        <HeaderBody>
          <p>Всього подій</p>
          <h1>{stats.totalEvents}</h1>
          <p>активний моніторинг</p>
        </HeaderBody>

        <HeaderBody>
          <p>Сьогодні</p>
          <h1>+{stats.todayEvents}</h1>
          <p>нових за добу</p>
        </HeaderBody>

        <HeaderBody>
          <p>Критичних</p>
          <h1>{stats.criticalEvents}</h1>
          <p>оцінка вище 80</p>
        </HeaderBody>

        <HeaderBody>
          <p>Країн</p>
          <h1>{stats.countriesCount}</h1>
          <p>у базі даних</p>
        </HeaderBody>

        <HeaderBody>
          <p>Середня оцінка</p>
          <h1>{stats.averagePriorityScore}</h1>
          <p>середній пріоритет</p>
        </HeaderBody>

        <HeaderBody>
          <p>Топ категорія</p>
          <h1>{stats.mostPopularCategory}</h1>
          <p>найбільше подій</p>
        </HeaderBody>

        <HeaderBody>
          <p>Найактивніша країна</p>
          <h1>{stats.mostActiveCountry}</h1>
          <p>за кількістю подій</p>
        </HeaderBody>
      </StatisticBlock>
    ))
  );
}

const StatisticBlock = styled.div`
  display: flex;
  flex-wrap: wrap;
  max-height: 90vh;
  /* flex-grow: 1; */
  width: auto;
  gap: 8px;
  ${HeaderBody} {
    flex: 1 1 auto;
    max-width: 20%;
    width: auto;
    flex-direction: column;
    gap: 0px;
    width: auto;
    padding: 16px 24px;
    p {
      color: var(--placeholder);
      font-size: 14px;
    }
    h1 {
      font-size: 22px;
      font-weight: 550;
      font-family: monospace;
      color: var(--text);
    }
  }
`;
