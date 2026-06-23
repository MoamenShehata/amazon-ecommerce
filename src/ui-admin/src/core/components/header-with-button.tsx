export function HeaderWithButton({ header, displayButton, onClick }: Readonly<any>) {
    let addProductButton = (
        <></>
    );

    if (displayButton) {
        addProductButton = (
            <>
                <button onClick={onClick} className="btn btn-success">
                    <i className="bi bi-plus"></i> Add New Product
                </button>
            </>
        );
    }

    return (
        <div className="d-flex justify-content-between align-items-center mb-4">
            <h2>{header}</h2>

            {addProductButton}

        </div>
    )
}