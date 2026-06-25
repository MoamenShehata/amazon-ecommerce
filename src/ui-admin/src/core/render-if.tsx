import type React from "react";

export default function RenderIf(props: React.PropsWithChildren<any>) {
  return props.flag && props.children;
}
