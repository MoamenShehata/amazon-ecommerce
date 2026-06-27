import React, { createContext, useContext } from "react";
import type { ThemeSettings } from "./theme-settings";

export const ThemeContext = createContext<
  [ThemeSettings, React.Dispatch<"Dark" | "Light">]
>([{ mode: "Dark" }, (x) => {}]);

export function useTheme() {
  return useContext(ThemeContext);
}
