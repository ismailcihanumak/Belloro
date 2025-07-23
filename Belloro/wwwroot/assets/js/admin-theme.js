/**
 * Belloro Admin Tema İşlevleri
 * 
 * Bu dosya, admin panelinde tema değiştirme işlevselliğini yönetir.
 * - Aydınlık/karanlık mod geçişleri
 * - Tema tercihlerinin yerel depolamada saklanması
 * - Sayfa yüklendiğinde son seçilen temanın uygulanması
 */

document.addEventListener('DOMContentLoaded', function () {
    // Tema butonu ve ilgili elemanlar
    const themeButton = document.getElementById('theme-button');
    const themeSwitch = document.getElementById('theme-switch');
    const body = document.body;

    // Yerel depolamadan tema tercihini al
    const getCurrentTheme = () => {
        return localStorage.getItem('belloro-theme') || 'dark'; // Varsayılan tema karanlık
    };

    // Tema tercihini yerel depolamaya kaydet
    const saveTheme = (theme) => {
        localStorage.setItem('belloro-theme', theme);
    };

    // Temayı uygula
    const applyTheme = (theme) => {
        if (theme === 'light') {
            body.classList.add('light-theme');
            themeButton.classList.remove('bx-moon');
            themeButton.classList.add('bx-sun');
        } else {
            body.classList.remove('light-theme');
            themeButton.classList.remove('bx-sun');
            themeButton.classList.add('bx-moon');
        }
    };

    // Tema geçişi
    const toggleTheme = () => {
        const currentTheme = getCurrentTheme();
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

        saveTheme(newTheme);
        applyTheme(newTheme);
    };

    // Sayfa yüklendiğinde son seçilen temayı uygula
    applyTheme(getCurrentTheme());

    // Tema butonuna tıklama olayını ekle - hem ikon için
    if (themeButton) {
        themeButton.addEventListener('click', function (e) {
            e.stopPropagation(); // Olayın themeSwitch'e yayılmasını önle
            toggleTheme();
        });
    }

    // Tema switch div'ine tıklama olayını ekle - tüm alan için
    if (themeSwitch) {
        themeSwitch.addEventListener('click', toggleTheme);
    }

    // Veritabanı verilerini simüle etmek için (gerçek uygulamada bu kısım olmayacak)
    updateDashboardStats();
});

// Veritabanından veri çekme simülasyonu
function updateDashboardStats() {
    // Bu fonksiyon normalde sunucuya AJAX isteği gönderip veritabanı verilerini çekecek
    // Şimdilik statik verilerle simülasyon yapıyoruz

    // Veritabanı verilerini simüle eden obje
    const dbData = {
        totalProducts: 152,
        totalCategories: 8,
        totalOrders: 38,
        totalCustomers: 24,
        recentOrders: [
            { id: 'ORD-0025', customer: 'Ahmet Yılmaz', product: 'Belloro Classic', amount: '₺3,450', status: 'Tamamlandı' },
            { id: 'ORD-0024', customer: 'Zeynep Aydın', product: 'Belloro Rose Gold', amount: '₺2,780', status: 'İşlemde' },
            { id: 'ORD-0023', customer: 'Mehmet Kaya', product: 'Belloro Sport', amount: '₺1,950', status: 'Kargoda' },
            { id: 'ORD-0022', customer: 'Ayşe Demir', product: 'Belloro Minimalist', amount: '₺2,100', status: 'Tamamlandı' },
            { id: 'ORD-0021', customer: 'Can Yıldız', product: 'Belloro Automatic', amount: '₺4,280', status: 'Tamamlandı' }
        ],
        topSellingProducts: [
            { name: 'Belloro Classic', sales: 48, image: '/assets/img/belloro-classic.jpg' },
            { name: 'Belloro Automatic', sales: 37, image: '/assets/img/belloro-automatic.jpg' },
            { name: 'Belloro Rose Gold', sales: 29, image: '/assets/img/belloro-rose-gold.jpg' }
        ]
    };

    // İstatistik rakamlarını güncelle
    updateStatValue('totalProducts', dbData.totalProducts);
    updateStatValue('totalCategories', dbData.totalCategories);
    updateStatValue('totalOrders', dbData.totalOrders);
    updateStatValue('totalCustomers', dbData.totalCustomers);

    // Son siparişler tablosunu güncelle
    updateRecentOrders(dbData.recentOrders);

    // En çok satan ürünleri güncelle
    updateTopSellingProducts(dbData.topSellingProducts);
}

// İstatistik değerini güncelleme yardımcı fonksiyonu
function updateStatValue(id, value) {
    const element = document.getElementById(id);
    if (element) {
        element.textContent = value;
    }
}

// Son siparişler tablosunu güncelleme
function updateRecentOrders(orders) {
    const tableBody = document.querySelector('.recent-orders-table tbody');
    if (!tableBody) return;

    tableBody.innerHTML = '';

    orders.forEach(order => {
        const statusClass = getStatusClass(order.status);

        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${order.id}</td>
            <td>${order.customer}</td>
            <td>${order.product}</td>
            <td>${order.amount}</td>
            <td><span class="badge ${statusClass}">${order.status}</span></td>
            <td>
                <button class="btn btn-action btn-view" title="Görüntüle"><i class="bx bx-show"></i></button>
                <button class="btn btn-action btn-edit" title="Düzenle"><i class="bx bx-edit"></i></button>
            </td>
        `;

        tableBody.appendChild(row);
    });
}

// En çok satan ürünleri güncelleme
function updateTopSellingProducts(products) {
    const container = document.querySelector('.top-selling-products');
    if (!container) return;

    container.innerHTML = '';

    products.forEach(product => {
        const productCard = document.createElement('div');
        productCard.className = 'watch-item';
        productCard.innerHTML = `
            <div class="product-image-container">
                <img src="${product.image}" alt="${product.name}" class="watch-image">
            </div>
            <h4 class="watch-title">${product.name}</h4>
            <div class="watch-sales">Satış: ${product.sales} adet</div>
        `;

        container.appendChild(productCard);
    });
}

// Sipariş durumuna göre sınıf adı döndürme
function getStatusClass(status) {
    switch (status) {
        case 'Tamamlandı':
            return 'badge-success';
        case 'İşlemde':
            return 'badge-warning';
        case 'Kargoda':
            return 'badge-info';
        default:
            return 'badge-secondary';
    }
}