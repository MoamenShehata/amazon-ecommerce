import { useAuth } from "oidc-react";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const auth = useAuth();
  const navigate = useNavigate();

  if (auth.isLoading) return <p>Loading...</p>;

  navigate("/catalog/products");
}
