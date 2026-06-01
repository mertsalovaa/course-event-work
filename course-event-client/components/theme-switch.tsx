"use client";

import { ThemeKey, useTheme } from "@/context/theme-context";

export const THEMES: Record<ThemeKey, { label: string }> = {
  dark: { label: "Темна" },
  light: { label: "Світла" },
};

export default function ThemeSwitcher() {
  const { theme, setTheme } = useTheme();

  return (
    <div
      style={{
        display: "flex",
        gap: 4,
        background: "var(--bg-secondary)",
        border: "0.5px solid var(--border)",
        borderRadius: 10,
        padding: 4,
        width: "auto"
      }}
    >
      {(Object.keys(THEMES) as ThemeKey[]).map((key) => (
        <button
          key={key}
          onClick={() => setTheme(key)}
          style={{
            padding: "6px 12px",
            borderRadius: 7,
            fontSize: 12,
            cursor: "pointer",
            fontFamily: "inherit",
            transition: "all 0.18s",
            fontWeight: theme === key ? 600 : 400,
            background: theme === key ? "var(--muted-teal)" : "transparent",
            color: theme === key ? "var(--background)" : "var(--muted-teal)",
            border: "1px solid var(--muted-teal)",
          }}
        >
          {THEMES[key].label}
        </button>
      ))}
    </div>
  );
}
