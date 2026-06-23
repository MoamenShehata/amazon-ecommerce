import type React from "react";

export default function Container({ children, classes }: Readonly<{
    children: React.ReactNode,
    classes: string
}>) {
    let className = "container";
    if (classes) className += " " + classes;
    return <div className={className}>{children}</div>;
}