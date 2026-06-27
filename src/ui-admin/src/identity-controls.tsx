import { useAuth } from "oidc-react";
import RenderIf from "./core/render-if";

export default function IdentityControls({}: Readonly<{
  isAuthenticated: boolean;
}>) {
  const auth = useAuth();

  const user = auth.userData?.profile;
  const isAuthenticated = user != null;

  function startOAuthLogin() {
    auth.signIn();
  }

  function logout() {
    auth.signOutRedirect();
  }

  return (
    <div className="navbar-nav ms-auto">
      <RenderIf flag={!isAuthenticated}>
        <li className="nav-link">
          <button
            className="btn btn-outline-light ms-2"
            type="button"
            onClick={startOAuthLogin}
          >
            Login
          </button>
        </li>
      </RenderIf>

      <RenderIf flag={isAuthenticated}>
        <li className="nav-link">
          <span>{user?.email}</span>
        </li>

        <li className="nav-item">
          {/* <a routerLink="/admin/orders" className="nav-link">All orders</a> */}
          <a className="nav-link">All orders</a>
        </li>

        <li className="nav-link">
          <button className="btn btn-outline-light ms-2" onClick={logout}>
            Logout
          </button>
        </li>
      </RenderIf>
    </div>
  );
}
