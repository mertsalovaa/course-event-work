import React, { Dispatch, SetStateAction } from "react";
import Header from "./header";
import { MainBody } from "@/app/events/[id]/page";
import { ViewType } from "@/app/api/entities.types";

export default function Layout({
  view,
  setView,
  children,
}: {
  view: ViewType;
  setView: Dispatch<SetStateAction<ViewType>>;
  children: React.ReactNode;
}) {
  return (
    <MainBody>
      <Header view={view} setView={setView} />
      {children}
    </MainBody>
  );
}
