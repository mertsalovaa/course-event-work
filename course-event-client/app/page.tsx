import EventsTab from "@/components/events-tab";
import { getEvents, getStatistic } from "@/app/api/events";
import Image from "next/image";
import { MainBody } from "./events/[id]/page";
import { EventDTO } from "./api/entities.types";

export default async function Home() {
  const { data, error } = await getEvents();
  console.log(data, error);
  if (error) {
    return (
      <MainBody>
        {error.type === "network"
          ? "⚠ Сервер недоступний"
          : `Помилка: ${error.message}`}
      </MainBody>
    );
  }

  if (!data || data.length === 0) {
    return <MainBody>Подій не знайдено</MainBody>;
  }

  const filteredData = data.filter(
    (e) =>
      e.latitude !== null &&
      e.longitude !== null &&
      Number.isFinite(Number(e.latitude)) &&
      Number.isFinite(Number(e.longitude)),
  );
  return <EventsTab events={filteredData} />;
}
