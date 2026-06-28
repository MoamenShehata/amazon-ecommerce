import axios from "axios";
import loadingService from "./core/services/loading.services";
import { useAuth } from "oidc-react";

export default function setUpAxiosGlobally() {
  // axios.defaults.headers.post["Content-Type"] = "application/json";

  axios.interceptors.request.use((request) => {
    const data = JSON.parse(
      sessionStorage.getItem(
        "oidc.user:https://localhost:5001:amazon.angular",
      )!,
    );

    if (data) {
      const accessToken = data["access_token"];
      request.headers.set("Authorization", `Bearer ${accessToken}`);
    }

    return request;
  });

  // axios.interceptors.request.use(
  //   (config) => {
  //     loadingService.show();
  //     return config;
  //   },
  //   (error) => {
  //     loadingService.hide();
  //     return Promise.reject(error);
  //   },
  // );

  // axios.interceptors.response.use(
  //   (response) => {
  //     loadingService.hide();
  //     return response;
  //   },
  //   (error) => {
  //     loadingService.hide();
  //     return Promise.reject(error);
  //   },
  // );
}
