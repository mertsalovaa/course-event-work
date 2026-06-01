import { CATEGORIES, useCategory } from "@/context/category-context";

export default function CategoryFilter({ view }: { view?: string }) {
  const { category, setCategory } = useCategory();

  return (
    <div
      style={{
        width: view === "map" ? "250px" : "100%",
        minWidth: "auto",
        maxWidth: view === "map" ? "300px" : "100%",
        display: "flex",
        gap: 6,
        padding: `8px 24px 8px ${view === "map" ? "0px" : "4px"}`,
        borderBottom: "0.5px solid rgba(255,255,255,0.05)",
        overflowX: "auto",
        paddingBottom: "8px",
        paddingTop: view === "map" ? "" : "8px",
      }}
    >
      {CATEGORIES.map((cat) => (
        <button
          key={cat}
          onClick={() => setCategory(cat)}
          style={{
            fontSize: 12,
            padding: "4px 12px",
            borderRadius: 20,
            cursor: "pointer",
            fontFamily: "inherit",
            letterSpacing: "0.03em",
            transition: "all 0.15s ease-in",
            background: "var(--background)",
            fontWeight: category === cat ? "550" : 400,
            color:
              category === cat ? "var(--bronze-spice)" : "var(--placeholder)",
            border: `0.5px solid ${category === cat ? "var(--bronze-spice)" : "rgba(255,255,255,0.1)"}`,
            boxShadow: "0 1px 3px 0px var(--boxShadow)",
          }}
        >
          {cat}
        </button>
      ))}
    </div>
  );
}
