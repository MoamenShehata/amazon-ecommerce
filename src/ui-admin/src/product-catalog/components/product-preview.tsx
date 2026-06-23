import type { ProductForListModel } from "./models/product-for-list-model";

export function ProductPreview({ product, onDeleted }: Readonly<{ product: ProductForListModel, onDeleted: () => void }>) {
    function deleteProduct() {
        onDeleted();
        // this.catalogService.deleteProduct(this.product.id)
        //   .subscribe(() => {
        //     this.deleted.emit();
        //   })
    }

    return (
        <div className="col-lg-3 col-md-4 col-sm-6 mb-4">
            <div className="card">
                {product.imageUrl != null
                    ? <img src={product.imageUrl} className="card-img-top" alt="..." style={{ padding: '40px' }} />
                    : <img src='assets/images/cubes.png' className="card-img-top" alt="..." style={{ padding: '40px' }} />
                }

                <h5 className="card-header">{product.name}</h5>

                <div className="card-body">
                    <p className="card-text mb-3">
                        <strong>Unit Price:</strong> ${product.unitPrice.toFixed(2)}
                    </p>
                    <span className="btn btn-primary mx-1" >
                        {product.categories.split(',').map(cat => cat)}
                    </span>
                </div>

                <div className="card-footer">
                    <button className="btn btn-danger" onClick={deleteProduct}>Delete</button>
                    {!product.isAvailable && <h4 className="text-danger">Currently Unavailable</h4>}

                </div >
            </div >
        </div >
    );
}