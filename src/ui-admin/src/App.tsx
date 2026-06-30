import { createBrowserRouter, Outlet, RouterProvider } from "react-router-dom";
import "./App.css";
import Container from "./core/bootstrap/components/bootstrap-container";
import LoadingSpinner from "./core/components/loading-spinner/loading-spinner";
import ProductList from "./product-catalog/components/products-list";
import Header from "./core/layout/header";
import CreateProduct from "./product-catalog/components/create-product/create-product";
import ThemeProvider from "./core/providers/theme-provider";
import { AuthProvider } from "oidc-react";
import { environment } from "./environment";
import Login from "./authentication/components/login";
import LoadingSpinnerContextProvider from "./core/components/loading-spinner/loading-context-provider";

function App() {
  const router = createBrowserRouter([
    {
      path: "/",
      element: (
        <LoadingSpinnerContextProvider>
          <LoadingSpinner />

          <ThemeProvider settings={{ mode: "Light" }}>
            <AuthProvider
              clientId="amazon.angular"
              authority={environment.authenticationBaseUrl}
              responseType="code"
              redirectUri={`${window.location.origin}/auth/login`}
              scope="openid profile email amazon.catalog amazon.cart amazon.customers"
              silentRedirectUri={`${window.location.origin}/silent-refresh.html`}
              postLogoutRedirectUri={window.location.origin}
            >
              <Header />

              <Container classes="my-5">
                <Outlet />
              </Container>
            </AuthProvider>
          </ThemeProvider>
        </LoadingSpinnerContextProvider>
      ),
      children: [
        {
          path: "auth/login",
          element: <Login />,
        },
        {
          path: "catalog",
          children: [
            {
              path: "products",
              element: <ProductList />,
            },
            {
              path: "products/create",
              element: <CreateProduct />,
            },
          ],
        },
      ],
    },
  ]);

  return <RouterProvider router={router} />;
}

export default App;
