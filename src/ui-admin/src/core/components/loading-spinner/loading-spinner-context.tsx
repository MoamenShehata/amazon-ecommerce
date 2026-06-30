import { createContext, useContext } from "react";

interface LoadingSpinnerContext {
  isLoading: boolean;
  show: () => void;
  hide: () => void;
}

export const LoadingSpinnerContext =
  createContext<LoadingSpinnerContext | null>(null);

export function useLoadingSpinner() {
  return useContext(LoadingSpinnerContext);
}
