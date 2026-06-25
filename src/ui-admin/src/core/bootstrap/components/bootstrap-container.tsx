import type { PropsWithChildren } from "react";

export default function Container(props: PropsWithChildren<any>) {
  let className = "container";
  if (props.classes) className += " " + props.classes;
  return <div className={className}>{props.children}</div>;
}
