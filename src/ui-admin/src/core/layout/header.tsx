import IdentityControls from "../../identity-controls";
import Container from "../bootstrap/components/bootstrap-container";
import Logo from "./logo";

export default function Header() {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
      <Container>
        <Logo />

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
  );
}
