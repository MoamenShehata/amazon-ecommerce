import React, { useState } from "react";
import type { ThemeSettings } from "./theme-settings";
import { ThemeContext } from "./theme.hook";

export default function ThemeProvider({
  settings,
  children,
}: Readonly<{ settings: ThemeSettings; children: React.ReactNode }>) {
  const [themeSettings, setThemeSettings] = useState<ThemeSettings>(
    settings || { mode: "Light" },
  );

  let classes =
    themeSettings.mode == "Light"
      ? "min-vh-100 bg-light"
      : "min-vh-100 bg-dark";

  return (
    <ThemeContext
      value={[
        themeSettings,
        (mode) => setThemeSettings({ ...themeSettings, mode: mode }),
      ]}
    >
      <div className={classes}>{children}</div>
    </ThemeContext>
  );
}
