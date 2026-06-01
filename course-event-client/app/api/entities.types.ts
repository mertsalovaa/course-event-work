export interface EventSource {
  sourceName: string;
  url: string;
  imageUrl: string;
  authorName: string;
  publishedAt: string;
}

export interface EventDTO {
  id: number;
  title: string;
  description: string;
  category: string;
  country: string;
  region: string;
  city: string;
  latitude: string;
  longitude: string;
  priorityScore: string;
  publishedAt: string;
  date: string;
  sources: EventSource[];
}

export interface DashboardStatsDTO {
  totalEvents: number;
  todayEvents: number;
  criticalEvents: number;
  countriesCount: number;
  averagePriorityScore: number;
  sourcesCount: number;
  mostPopularCategory: string;
  mostActiveCountry: string;
}

export interface ImportResult {
  newImportedCount: number;
  importedAt: string;
}

export type ViewType = "map" | "list";

export type ShowToastType = "success" | "error" | "info";

export interface ImageType {
  color?: string;
  size?: number;
}
