"use client";

import { createContext, useContext, useEffect, useState } from "react";

export type ThemeKey = "dark" | "light";

interface ThemeCtx {
  theme: ThemeKey;
  setTheme: (t: ThemeKey) => void;
}

const ThemeContext = createContext<ThemeCtx>({
  theme: "dark",
  setTheme: () => {},
});

export function ThemeProvider({ children }: { children: React.ReactNode }) {
  const [theme, setThemeState] = useState<ThemeKey>("dark");

  const applyTheme = (t: ThemeKey) => {
    document.documentElement.setAttribute("data-theme", t);
    localStorage.setItem("theme", t);
    setThemeState(t);
  };

  // Читаємо з localStorage при завантаженні
  useEffect(() => {
    const saved = localStorage.getItem("theme") as ThemeKey | null;
    // eslint-disable-next-line react-hooks/set-state-in-effect
    if (saved) applyTheme(saved);
  }, []);

  return (
    <ThemeContext.Provider value={{ theme, setTheme: applyTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

export const useTheme = () => useContext(ThemeContext);
