
CREATE SCHEMA IF NOT EXISTS `weak` DEFAULT CHARACTER SET utf8mb4 ;
USE `weak` ;

SET global log_bin_trust_function_creators=1;
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


CREATE TABLE `members` (
  `id` int(10) UNSIGNED NOT NULL COMMENT 'メンバーID',
  `name` varchar(20) NOT NULL COMMENT '氏名',
  `deleted_flag` tinyint(1) NOT NULL DEFAULT 0,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='メンバー';

ALTER TABLE `members`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `members`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'メンバー';


INSERT INTO members (name, deleted_flag) VALUES ('鈴木 太郎', 0);
INSERT INTO members (name, deleted_flag) VALUES ('鈴木 次郎', 0);
INSERT INTO members (name, deleted_flag) VALUES ('鈴木 三郎', 0);
INSERT INTO members (name, deleted_flag) VALUES ('鈴木 四郎', 1);
INSERT INTO members (name, deleted_flag) VALUES ('鈴木 五郎', 1);
INSERT INTO members (name, deleted_flag) VALUES ('鈴木 六郎', 1);



CREATE TABLE `board_xss` (
  `id` int(10) UNSIGNED NOT NULL COMMENT '掲示板XSSID',
  `name` varchar(100) NOT NULL COMMENT '名前',
  `comment` text NOT NULL COMMENT 'コメント',
  `deleted_flag` tinyint(1) NOT NULL DEFAULT 0,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='メンバー';


ALTER TABLE `board_xss`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `board_xss`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '掲示板XSS';
  
INSERT INTO board_xss (name, comment) VALUES ('ユーザー1', 'コメント1');
INSERT INTO board_xss (name, comment) VALUES ('ユーザー2', 'コメント2');
INSERT INTO board_xss (name, comment) VALUES ('ユーザー3', 'コメント3');
INSERT INTO board_xss (name, comment) VALUES ('ユーザー4', 'コメント4');
INSERT INTO board_xss (name, comment) VALUES ('ユーザー5', 'コメント5');
INSERT INTO board_xss (name, comment) VALUES ('ユーザー6', 'コメント6');


CREATE TABLE board_csrf LIKE board_xss;
INSERT INTO board_csrf SELECT * FROM board_xss;


ALTER TABLE `board_csrf`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT '掲示板XSS';