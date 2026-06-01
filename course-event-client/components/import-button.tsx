"use client";

import { useState, useTransition } from "react";
import { useRouter } from "next/navigation";
import { ArrowDownIcon } from "@/images/icons/arrow-down-icon";
import { ShowToastType } from "@/app/api/entities.types";

export default function ImportButton() {
  const router = useRouter();
  const [isPending, startTransition] = useTransition();
  const [toast, setToast] = useState<{
    type: ShowToastType;
    message: string;
  } | null>(null);
  const [newCount, setNewCount] = useState<number | null>(null);

  const showToast = (type: ShowToastType, message: string, duration: number = 4000) => {
    setToast({ type, message });
    setTimeout(() => setToast(null), duration ?? 4000);
  };

  const handleImport = async () => {
    try {
      showToast("info", "Імпорт та аналіз подій запущено...");
      const res = await fetch("/api/import", {
        method: "POST",
      });

      const result = await res.json();

      if (result.newImportedCount > 0) {
        showToast(
          "success",
          `Імпортовано ${result.newImportedCount} нових подій`,
        );
      } else {
        showToast("info", "Нових подій не знайдено");
      }
    } catch {
      showToast("error", "Помилка імпорту");
    }
  };

  return (
    <>
      {/* Лічильник нових подій */}
      {newCount !== null && newCount > 0 && (
        <div
          style={{
            display: "flex",
            alignItems: "center",
            gap: 6,
            padding: "4px 12px",
            borderRadius: 20,
            background: "transparent",
            border: "0.5px solid var(--muted-teal)",
            fontFamily: "monospace",
            fontSize: 11,
            color: "var(--muted-teal)",
            animation: "fadeIn 0.3s ease",
          }}
        >
          <div
            style={{
              width: 6,
              height: 6,
              borderRadius: "50%",
              background: "var(--muted-teal)",
              boxShadow: "0 0 6px var(--muted-teal)",
            }}
          />
          +{newCount} нових
        </div>
      )}

      {/* Кнопка */}
      <button
        onClick={handleImport}
        disabled={isPending}
        className="hover:opacity-75 flex gap-2"
        style={{
          display: "flex",
          alignItems: "center",
          gap: 7,
          padding: "7px 16px",
          borderRadius: 8,
          cursor: isPending ? "not-allowed" : "pointer",
          background: "var(--background)",
          border: `1px solid var(--text)`,
          color: "var(--text)",
          fontFamily: "inherit",
          fontSize: 13,
          fontWeight: 500,
          transition: "all 0.18s",
        }}
      >
        {isPending ? (
          <>
            <span
              style={{
                display: "inline-block",
                width: 12,
                height: 12,
                border: "1.5px solid var(--background)",
                borderTopColor: "var(--muted-teal)",
                borderRadius: "50%",
                animation: "spin 0.7s linear infinite",
              }}
            />
            Імпортую...
          </>
        ) : (
          <>
            <ArrowDownIcon color={"var(--text)"} /> Оновити події
          </>
        )}
      </button>

      {/* Toast */}
      {toast && (
        <div
          style={{
            position: "fixed",
            bottom: 24,
            right: 24,
            zIndex: 9999,
            display: "flex",
            alignItems: "center",
            gap: 10,
            padding: "12px 18px",
            borderRadius: 10,
            background: "var(--text)",
            border: `0.5px solid ${toast.type === "success" ? "#3dd68c44" : toast.type === "info" ? "#3d5ed644" : "#e0555544"}`,
            boxShadow: "0 8px 32px rgba(0,0,0,0.5)",
            fontFamily: "inherit",
            fontSize: 13,
            color: "var(--background)",
            animation: "slideUp 0.3s ease",
          }}
        >
          <span style={{ fontSize: 16 }}>
            {toast.type === "success" ? "✓" : toast.type === "info" ? "?" : "✕"}
          </span>
          {toast.message}
        </div>
      )}

      <style>{`
        @keyframes spin    { to { transform: rotate(360deg); } }
        @keyframes slideUp { from { transform: translateY(12px); opacity: 0; } to { transform: translateY(0); opacity: 1; } }
        @keyframes fadeIn  { from { opacity: 0; } to { opacity: 1; } }
      `}</style>
    </>
  );
}
