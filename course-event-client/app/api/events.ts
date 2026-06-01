import { API_BASE_EVENTS } from "./constants";
import { DashboardStatsDTO, EventDTO, ImportResult } from "./entities.types";
import { apiFetch } from "./fetch";

export const getStatistic = async () =>
  apiFetch<DashboardStatsDTO>(`${API_BASE_EVENTS}/dashboard`, {
    next: { revalidate: 60 },
  });

export const getEvents = () =>
  apiFetch<EventDTO[]>(`${API_BASE_EVENTS}`, {
    next: { revalidate: 300 },
  });

export const getEventById = (id: string) =>
  apiFetch<EventDTO>(`${API_BASE_EVENTS}/${id}`, {
    next: { revalidate: 600 },
  });

export const triggerImport = () =>
  apiFetch<ImportResult>(`${API_BASE_EVENTS}/import`, {
    method: "POST",
    cache: "no-store",
  });
