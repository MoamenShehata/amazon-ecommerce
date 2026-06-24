import axios from "axios";
import loadingService from "./core/services/loading.services";

export default function setUpAxiosGlobally() {
  axios.defaults.headers.post["Content-Type"] = "application/json";

  axios.interceptors.request.use(
    (config) => {
      loadingService.show();
      return config;
    },
    (error) => {
      loadingService.hide();
      return Promise.reject(error);
    }
  );

  axios.interceptors.response.use(
    (response) => {
      loadingService.hide();
      return response;
    },
    (error) => {
      loadingService.hide();
      return Promise.reject(error);
    }
  );
}
