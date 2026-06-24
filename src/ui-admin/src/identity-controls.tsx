import RenderIf from "./core/render-if";

export default function IdentityControls({
  isAuthenticated,
}: Readonly<{ isAuthenticated: boolean }>) {
  function startOAuthLogin() {}
  function logout() {}

  return (
    <div className="navbar-nav ms-auto">
      <RenderIf
        flag={!isAuthenticated}
        component={
          <li className="nav-link">
            <button
              className="btn btn-outline-light ms-2"
              type="button"
              onClick={startOAuthLogin}
            >
              Login
            </button>
          </li>
        }
      />

      <RenderIf
        flag={isAuthenticated}
        component={
          <>
            <li className="nav-link">
              <span>Email</span>
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
          </>
        }
      />
    </div>
  );
}
