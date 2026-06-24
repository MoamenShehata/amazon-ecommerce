import type React from "react";

export default function RenderIf({
  flag,
  component,
}: Readonly<{ flag: boolean; component: React.ReactNode }>) {
  return flag && component;
}
