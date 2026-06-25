import "./App.css";
import Container from "./core/bootstrap/components/bootstrap-container";
import LoadingSpinner from "./core/components/loading-spinner/loading-spinner";
import IdentityControls from "./identity-controls";
import ProductList from "./product-catalog/components/products-list";

function App() {
  return (
    <>
      <LoadingSpinner />
      <div className="min-vh-100 bg-light">
        <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
          <Container>
            <a className="navbar-brand fw-bold" href="#">
              Amazon
            </a>

            <button
              className="navbar-toggler"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#navbarNav"
              aria-controls="navbarNav"
              aria-expanded="false"
              aria-label="Toggle navigation"
            >
              <span className="navbar-toggler-icon"></span>
            </button>

            <div className="collapse navbar-collapse" id="navbarNav">
              <IdentityControls isAuthenticated={false} />
            </div>
          </Container>
        </nav>

        <Container classes="my-5">
          <ProductList />
          {/* <router-outlet></router-outlet> */}
        </Container>
      </div>
    </>
  );
}

export default App;
