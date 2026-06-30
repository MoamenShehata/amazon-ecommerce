import axios from "axios";

let showLoading: (() => void) | null = null;
let hideLoading: (() => void) | null = null;

export const registerLoadingHandlers = (show: () => void, hide: () => void) => {
  showLoading = show;
  hideLoading = hide;
};

export default function setUpAxiosGlobally() {
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

  axios.interceptors.request.use(
    (config) => {
      debugger;
      showLoading?.();
      return config;
    },
    (error) => {
      hideLoading?.();
      return Promise.reject(error);
    },
  );

  axios.interceptors.response.use(
    (response) => {
      hideLoading?.();
      return response;
    },
    (error) => {
      hideLoading?.();
      return Promise.reject(error);
    },
  );
}
