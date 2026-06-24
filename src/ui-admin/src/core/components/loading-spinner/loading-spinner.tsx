import loadingService from "../../services/loading.services";

export default function LoadingSpinner() {
    return !loadingService.loading ? <></> : (
        <div className="overlay">
            <div className="spinner"></div>
        </div >
    );
}