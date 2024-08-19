# Kuyruk Simulation

Kuyruk Simulation, otobüs duraklarındaki kuyrukların simülasyonunu gerçekleştiren bir WPF uygulamasıdır. Bu proje, veritabanındaki kuyruk uzunluklarını görselleştirerek gerçek zamanlı bir simülasyon sağlar ve otobüs durakları ile kuyruk girişlerini yönetmek için çeşitli işlevler sunar.

<p align="center">
  <img src="ScreenShoots/1.png" width="500" />
  <img src="ScreenShoots/2.png" width="500" />
</p>

<p align="center">
  <img src="ScreenShoots/3.png" width="500" />
  <img src="ScreenShoots/4.png" width="500" />
</p>

<p align="center">
  <img src="ScreenShoots/5.png" width="500" />
  <img src="ScreenShoots/6.png" width="500" />
</p>


## Özellikler

- **Otobüs Durağı Ekleme**: Yeni otobüs duraklarını veritabanına ekleme yeteneği.
- **Kuyruk Girişi Ekleme**: Seçilen otobüs durağına kuyruk uzunluğu ekleme işlemi.
- **Veri Yükleme ve Görselleştirme**: Otobüs duraklarındaki kuyruk uzunluklarını veritabanından yükler ve görsel olarak sunar.
- **Dinamik Daireler**: Kuyruk uzunluklarına göre dairelerin boyutunu dinamik olarak ayarlar.
- **Zamanlayıcı ve Hız Kontrolü**: Simülasyon hızını değiştirme yeteneği sağlar. Kullanıcı, simülasyon hızını 0.25x, 0.50x, 1x, 1.25x ve 2x olarak ayarlayabilir.
- **İnteraktif UI**: Kullanıcı dostu bir arayüz ile simülasyonun başlangıcı ve hızı kolayca kontrol edilebilir.

## Veri Tabanı Detayları

- **Otobüs Durakları**: Veritabanında toplamda 50 adet otobüs durağı bulunmaktadır.
- **Kuyruk Verileri**: Her otobüs durağı için 100.000 adet kuyruk uzunluğu verisi bulunmaktadır, bu da toplamda 5 milyon veri kaydını ifade eder.

## Teknolojiler

- **.NET Framework**: WPF uygulaması için kullanılan temel teknoloji.
- **Entity Framework Core**: Veritabanı işlemleri ve veri erişimi için kullanılır.
- **XAML**: Kullanıcı arayüzü tasarımı için kullanılır.
- **C#**: Uygulama mantığını yazmak için kullanılan programlama dili.


