import { useLoadingSpinner } from "./loading-spinner-context";
import classes from "./loading-spinner.module.css";

export default function LoadingSpinner() {
  const spinnerContext = useLoadingSpinner()!;
  debugger;
  return !spinnerContext.isLoading ? (
    <></>
  ) : (
    <div className={classes.overlay}>
      <div className={classes.spinner}></div>
    </div>
  );
}
