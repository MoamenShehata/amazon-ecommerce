import { createBrowserRouter, Outlet, RouterProvider } from "react-router-dom";
import "./App.css";
import Container from "./core/bootstrap/components/bootstrap-container";
import LoadingSpinner from "./core/components/loading-spinner/loading-spinner";
import IdentityControls from "./identity-controls";
import ProductList from "./product-catalog/components/products-list";
import Header from "./core/layout/header";

function App() {
  const router = createBrowserRouter([
    {
      path: "/",
      element: (
        <>
          <LoadingSpinner />

          <div className="min-vh-100 bg-light">
            <Header />

            <Container classes="my-5">
              <Outlet />
            </Container>
          </div>
        </>
      ),
      children: [
        {
          path: "products",
          element: <ProductList />,
        },
      ],
    },
  ]);

  return <RouterProvider router={router} />;
}

export default App;
