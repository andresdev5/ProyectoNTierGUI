-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: proyecto_ntier
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `account_types`
--

DROP TABLE IF EXISTS `account_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_types` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_types`
--

LOCK TABLES `account_types` WRITE;
/*!40000 ALTER TABLE `account_types` DISABLE KEYS */;
INSERT INTO `account_types` VALUES (2,'CAPITAL','2023-07-19 01:16:57'),(3,'PASIVO','2023-07-19 01:22:04'),(4,'ACTIVO','2023-07-19 02:05:19'),(6,'EGRESO','2023-07-19 02:06:26'),(7,'CAPITAL','2023-07-19 02:06:29'),(12,'INGRESO','2023-07-19 03:19:13');
/*!40000 ALTER TABLE `account_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accounting_entries`
--

DROP TABLE IF EXISTS `accounting_entries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounting_entries` (
  `id` int NOT NULL AUTO_INCREMENT,
  `employee_id` int NOT NULL,
  `debit_sum` double NOT NULL DEFAULT '0',
  `credit_sum` double NOT NULL DEFAULT '0',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `accounting_entry_employee_fk_idx` (`employee_id`),
  CONSTRAINT `accounting_entry_employee_fk` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounting_entries`
--

LOCK TABLES `accounting_entries` WRITE;
/*!40000 ALTER TABLE `accounting_entries` DISABLE KEYS */;
INSERT INTO `accounting_entries` VALUES (6,3,20,0,'2023-07-16 23:52:05'),(7,1,110,0,'2023-07-16 23:54:18'),(8,1,110,0,'2023-07-16 23:56:24'),(9,1,50,100,'2023-07-17 00:02:00');
/*!40000 ALTER TABLE `accounting_entries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accounting_entry_details`
--

DROP TABLE IF EXISTS `accounting_entry_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounting_entry_details` (
  `id` int NOT NULL AUTO_INCREMENT,
  `accounting_entry_id` int NOT NULL,
  `transaction_reason_id` int NOT NULL,
  `debit` double NOT NULL DEFAULT '0',
  `credit` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `accounting_entries_detail_id_fk_idx` (`accounting_entry_id`),
  KEY `accounting_entry_detail_transaction_reason_fk_idx` (`transaction_reason_id`),
  CONSTRAINT `accounting_entries_detail_id_fk` FOREIGN KEY (`accounting_entry_id`) REFERENCES `accounting_entries` (`id`),
  CONSTRAINT `accounting_entry_detail_transaction_reason_fk` FOREIGN KEY (`transaction_reason_id`) REFERENCES `transaction_reasons` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounting_entry_details`
--

LOCK TABLES `accounting_entry_details` WRITE;
/*!40000 ALTER TABLE `accounting_entry_details` DISABLE KEYS */;
INSERT INTO `accounting_entry_details` VALUES (2,6,11,20,0),(3,7,2,3,0),(4,8,3,10,0),(5,8,4,0,0),(6,8,7,0,0),(7,8,8,0,0),(8,8,9,100,0),(9,8,10,0,0),(10,9,12,0,100),(11,9,13,50,0);
/*!40000 ALTER TABLE `accounting_entry_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounts` (
  `id` int NOT NULL AUTO_INCREMENT,
  `account_type_id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (1,4,'CUENTA PRUEBA','2023-07-19 03:16:29'),(2,12,'CUENTA INGRESO','2023-07-19 03:22:59');
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employees` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_card` varchar(15) DEFAULT NULL,
  `full_name` varchar(255) NOT NULL,
  `hire_date` datetime NOT NULL,
  `salary` double NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (1,'1726744293','John','2023-07-12 00:00:00',2000,'2023-07-12 07:19:04',NULL),(3,'1726744294','Paul Doe','2023-07-16 00:00:00',5000,'2023-07-16 22:49:33',NULL);
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transaction_reasons`
--

DROP TABLE IF EXISTS `transaction_reasons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction_reasons` (
  `id` int NOT NULL AUTO_INCREMENT,
  `employee_id` int DEFAULT NULL,
  `type` varchar(45) NOT NULL,
  `reason` varchar(255) NOT NULL,
  `amount` double NOT NULL DEFAULT '0',
  `checked` tinyint NOT NULL DEFAULT '0',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction_reasons`
--

LOCK TABLES `transaction_reasons` WRITE;
/*!40000 ALTER TABLE `transaction_reasons` DISABLE KEYS */;
INSERT INTO `transaction_reasons` VALUES (1,1,'EGRESO','TEST1!',0,1,'2023-07-12 04:50:19',NULL),(2,1,'INGRESO','1',0,1,'2023-07-12 05:14:54',NULL),(3,1,'EGRESO','test',10,1,'2023-07-12 05:16:04',NULL),(4,1,'INGRESO','dasdas',0,1,'2023-07-12 05:20:34',NULL),(7,1,'INGRESO','dasdas',0,1,'2023-07-12 05:20:34',NULL),(8,1,'INGRESO','abc',0,1,'2023-07-12 05:36:38',NULL),(9,1,'EGRESO','TEST9!',100,1,'2023-07-12 05:36:48',NULL),(10,1,'EGRESO','liquidacion',0,1,'2023-07-12 20:59:07',NULL),(11,3,'EGRESO','abcd',20,1,'2023-07-16 22:48:45',NULL),(12,1,'INGRESO','MOTIVOA',100,1,'2023-07-17 05:01:24',NULL),(13,1,'EGRESO','MOTIVOB',50,1,'2023-07-17 05:01:47',NULL);
/*!40000 ALTER TABLE `transaction_reasons` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-07-19  3:30:25
