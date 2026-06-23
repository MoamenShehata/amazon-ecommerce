import type React from "react";

export function MayBeEmptyList({ list, component }: Readonly<{ list: any[], component: React.ReactNode }>) {
    return !list || list.length == 0
        ? <div className="alert alert-warning" role="alert">No Data found.</div>
        : component;
}