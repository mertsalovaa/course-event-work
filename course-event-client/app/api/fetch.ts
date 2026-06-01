import https from "https";
import { ApiResult } from "./types";

const devAgent = new https.Agent({ rejectUnauthorized: false });

export const apiFetchRaw = (url: string, options?: RequestInit) =>
  fetch(url, {
    ...options,
    // @ts-expect-error — agent не є стандартним полем fetch, але Node.js його підтримує
    agent: process.env.NODE_ENV === "development" ? devAgent : undefined,
  });

export async function apiFetch<T>(
  url: string,
  options?: RequestInit,
): Promise<ApiResult<T>> {
  try {
    const response = await apiFetchRaw(url, options); // твій існуючий apiFetch

    if (!response.ok) {
      let message = `HTTP ${response.status}`;
      try {
        const b = await response.json();
        message = b.message ?? b.title ?? message;
      } catch {}
      return {
        data: null,
        error: { message, status: response.status, type: "server" },
      };
    }

    try {
      return { data: await response.json(), error: null };
    } catch {
      return { data: null, error: { message: "Parse error", type: "parse" } };
    }
  } catch (err) {
    return {
      data: null,
      error: {
        message: err instanceof Error ? err.message : "Unknown",
        type: "network",
      },
    };
  }
}
