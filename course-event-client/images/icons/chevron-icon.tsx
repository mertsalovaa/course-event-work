import { ImageType } from "@/app/api/entities.types";

export const ChevronIcon = ({
  size = 24,
  color = "var(--text)",
  direction = "right",
}: ImageType & {
  direction?: "right" | "left" | "up" | "down";
}) => {
  const rotation = {
    right: "0deg",
    down: "90deg",
    left: "180deg",
    up: "270deg",
  }[direction];

  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      style={{ transform: `rotate(${rotation})` }}
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M9.29303 18.707C9.10556 18.5195 9.00024 18.2652 9.00024 18C9.00024 17.7348 9.10556 17.4805 9.29303 17.293L14.586 12L9.29303 6.70701C9.11087 6.51841 9.01008 6.26581 9.01236 6.00361C9.01464 5.74141 9.1198 5.4906 9.30521 5.30519C9.49062 5.11978 9.74143 5.01461 10.0036 5.01234C10.2658 5.01006 10.5184 5.11085 10.707 5.29301L16.707 11.293C16.8945 11.4805 16.9998 11.7348 16.9998 12C16.9998 12.2652 16.8945 12.5195 16.707 12.707L10.707 18.707C10.5195 18.8945 10.2652 18.9998 10 18.9998C9.73487 18.9998 9.48056 18.8945 9.29303 18.707Z"
        fill={color}
      />
    </svg>
  );
};
