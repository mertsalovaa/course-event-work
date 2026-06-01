export type ApiResult<T> =
  | { data: T; error: null }
  | { data: null; error: ApiError };

export interface ApiError {
  message: string;
  status?: number;
  type: "network" | "server" | "parse" | "unknown";
}
