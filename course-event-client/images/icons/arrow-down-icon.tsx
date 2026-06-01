import { ImageType } from "@/app/api/entities.types";

export const ArrowDownIcon = ({
  color = "var(--text)",
  size = 20,
}: ImageType) => {
  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M12 19V5M16 15L12 19L8 15"
        stroke={color}
        strokeWidth="2"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  );
};
