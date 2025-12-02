import { BrowserRouter } from "react-router-dom";
import AppRoutes from "./routes/AppRoutes";
import { ProvideContext } from "./Auth/Auth";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";

function App() {
    return (
        <ProvideContext>
            <BrowserRouter>
                <AppRoutes />
            </BrowserRouter>
        </ProvideContext>
    );
}

export default App;
