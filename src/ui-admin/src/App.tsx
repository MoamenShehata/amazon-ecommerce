import './App.css'
import LoadingSpinner from './core/components/loading-spinner/loading-spinner'
import ProductList from './product-catalog/components/products-list'

function App() {

  return (
    <>
      <LoadingSpinner />
      <ProductList />
    </>
  )
}

export default App
