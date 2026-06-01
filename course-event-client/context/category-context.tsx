"use client";

import { createContext, useContext, useEffect, useState } from "react";

export const CATEGORIES = [
  "Всі",
  "Політика",
  "Економіка",
  "Технології",
  "Наука",
  "Здоров'я",
  "Екологія",
  "Катастрофа",
  "Спорт",
  "Розваги",
  "Безпека",
  "Військове",
  "Фінанси",
  "Інше",
];

interface CategoryCtx {
  category: string;
  setCategory: (t: string) => void;
}

const CategoryContext = createContext<CategoryCtx>({
  category: "Всі",
  setCategory: () => {},
});

export function CategoryProvider({ children }: { children: React.ReactNode }) {
  const [category, setCategory] = useState<string>("Всі");

  const applyCategory = (t: string) => {
    localStorage.setItem("active-category", t);
    setCategory(t);
  };

  // Читаємо з localStorage при завантаженні
  useEffect(() => {
    const saved = localStorage.getItem("active-category") as string | "Всі";
    // eslint-disable-next-line react-hooks/set-state-in-effect
    if (saved) applyCategory(saved);
  }, []);

  return (
    <CategoryContext.Provider value={{ category, setCategory: applyCategory }}>
      {children}
    </CategoryContext.Provider>
  );
}

export const useCategory = () => useContext(CategoryContext);
