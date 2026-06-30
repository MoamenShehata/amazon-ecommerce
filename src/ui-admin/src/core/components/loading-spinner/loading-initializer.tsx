import { useEffect } from "react";
import { registerLoadingHandlers } from "../../../axios.setup";
import { useLoadingSpinner } from "./loading-spinner-context";

export function LoadingInitializer() {
  const { show, hide } = useLoadingSpinner()!;

  useEffect(() => {
    registerLoadingHandlers(show, hide);
  }, [show, hide]);
  return <></>;
}
