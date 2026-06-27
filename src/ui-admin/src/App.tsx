import { createBrowserRouter, Outlet, RouterProvider } from "react-router-dom";
import "./App.css";
import Container from "./core/bootstrap/components/bootstrap-container";
import LoadingSpinner from "./core/components/loading-spinner/loading-spinner";
import ProductList from "./product-catalog/components/products-list";
import Header from "./core/layout/header";
import CreateProduct from "./product-catalog/components/create-product/create-product";
import ThemeProvider from "./core/providers/theme-provider";

function App() {
  const router = createBrowserRouter([
    {
      path: "/",
      element: (
        <>
          <LoadingSpinner />

          <ThemeProvider settings={{ mode: "Light" }}>
            <Header />

            <Container classes="my-5">
              <Outlet />
            </Container>
          </ThemeProvider>
        </>
      ),
      children: [
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
