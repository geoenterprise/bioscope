Moises Parra Lozano's Quote: "And there were some who died with fevers, which at some seasons of the year were very frequent in the land—but not so much so with fevers, because of the excellent qualities of the many plants and roots which God had prepared to remove the cause of diseases, to which men were subject by the nature of the climate." Alma 46:40

Jovanny Rey's Quote: "And if men come unto me I will show unto them their weakness. I give unto men weakness that they may be humble; and my grace is sufficient for all men that humble themselves before me; for if they humble themselves before me, and have faith in me, then will I make weak things become strong unto them."

# 🌿 Bioscope

**Bioscope** is an app currently in development that helps you explore and learn about the world of plants.

## 🌱 Key Features

- 📸 **AI Plant Identification**  
  Take a photo of your favorite plant and let the AI help you identify it. Bioscope provides suggestions for possible identifications based on your photo.

- 🔍 **Detailed Information**  
  View each suggestion’s details — including its **scientific name**, **common name**, **characteristics**, and a **photo**.

- 💾 **Save Your Discoveries**  
  Save identified plants to review them anytime.

- ✏️ **Manage Your Discoveries**  
  Add comments, update plant characteristics, or delete your discoveries easily.

---

## ⚙️ Running the Application with USB Debugger

The application can be deployed to an Android device using **USB Debugger**.  

### 1️⃣ Install MAUI and Android workloads (if first time)
>>dotnet workload install maui
>>dotnet workload install android

2️⃣ Restore project packages for additional packages
>>dotnet restore

3️⃣ Configure your Android device
Enable Developer Mode on your device.

Connect it via USB.

Check if your device is detected:
>>adb devices

4️⃣ Run the application
>>dotnet maui run -f net8.0-android

Note:
The API that the MAUI project consumes (Bioscope) is already hosted remotely on Render.
