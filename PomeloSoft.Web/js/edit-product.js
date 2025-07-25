document.addEventListener('DOMContentLoaded', () => {
    const apiBaseUrl = 'http://localhost:5008/api'; 
    const form = document.getElementById('edit-product-form');
    const categorySelect = document.getElementById('category');
    const notificationArea = document.getElementById('form-notification');
    
    const params = new URLSearchParams(window.location.search);
    const productId = params.get('id');

    function clearErrors() {
        document.querySelectorAll('.error-message').forEach(el => el.textContent = '');
        notificationArea.style.display = 'none';
        notificationArea.textContent = '';
        notificationArea.className = 'notification';
    }
    function displayErrors(errors) {
        for (const key in errors) {
            const inputName = key.charAt(0).toLowerCase() + key.slice(1);
            const errorDiv = form.querySelector(`[name="${inputName}"] + .error-message`);
            if (errorDiv) {
                errorDiv.textContent = errors[key].join(' ');
            }
        }
    }
    function showNotification(message, type = 'error') {
        notificationArea.textContent = message;
        notificationArea.className = `notification ${type}`;
        notificationArea.style.display = 'block';
    }

    if (!productId) {
        showNotification('Product ID not found for editing.');
        return;
    }

    async function loadInitialData() {
        try {
            const [categoriesResponse, productResponse] = await Promise.all([
                fetch(`${apiBaseUrl}/categories`),
                fetch(`${apiBaseUrl}/products/${productId}`)
            ]);

            if (!categoriesResponse.ok) throw new Error('Kategoriler yüklenemedi.');
            if (!productResponse.ok) throw new Error('Ürün bilgileri yüklenemedi.');

            const categories = await categoriesResponse.json();
            const product = await productResponse.json();

            // Kategorileri dropdown'a doldur
            categorySelect.innerHTML = '';
            categories.forEach(cat => {
                const option = document.createElement('option');
                option.value = cat.id;
                option.textContent = cat.name;
                categorySelect.appendChild(option);
            });

            form.name.value = product.name;
            form.brand.value = product.brand;
            form.categoryId.value = product.categoryId;
            form.price.value = product.price;
            form.stock.value = product.stock;
            form.imageThumbnailUrl.value = product.imageThumbnailUrl;
            form.description.value = product.description;

        } catch (error) {
            console.error(error);
            alert('Veriler yüklenirken bir hata oluştu: ' + error.message);
        }
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        clearErrors();

        const formData = new FormData(form);
        const productData = Object.fromEntries(formData.entries());

        productData.price = parseFloat(productData.price);
        productData.stock = parseInt(productData.stock, 10);
        productData.categoryId = parseInt(productData.categoryId, 10);

        try {
            const response = await fetch(`${apiBaseUrl}/products/${productId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(productData)
            });

            if (response.ok) {
                sessionStorage.setItem('successMessage', `Product updated successfully!`);
                window.location.href = 'index.html';
            } else {
                const contentType = response.headers.get("content-type");
                let errorData;
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    errorData = await response.json();
                } else {
                    errorData = await response.text();
                }

                if (errorData.errors) {
                    displayErrors(errorData.errors);
                } else {
                    showNotification(errorData);
                }
            }
        } catch (error) {
            console.error('Request error:', error);
            showNotification('An error occurred while connecting to the server.');
        }
    });

    loadInitialData();
}); 