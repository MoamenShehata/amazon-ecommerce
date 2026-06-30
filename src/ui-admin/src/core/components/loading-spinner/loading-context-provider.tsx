import type React from "react";
import { useState } from "react";
import { LoadingInitializer } from "./loading-initializer";
import { LoadingSpinnerContext } from "./loading-spinner-context";

export default function LoadingSpinnerContextProvider({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  const [requestsCount, setRequestsCount] = useState(0);

  return (
    <LoadingSpinnerContext
      value={{
        isLoading: requestsCount > 0,
        show: () => setRequestsCount((x) => x + 1),
        hide: () => setRequestsCount((x) => Math.max(0, x - 1)),
      }}
    >
      <LoadingInitializer />
      {children}
    </LoadingSpinnerContext>
  );
}
