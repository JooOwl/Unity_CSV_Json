using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace AspNetServer.DBInterface
{
    public class User
    {
        public readonly string Name;

        public User(string name)
        {
            Name = name;
        }
    }

    public class SimpleGameDB
    {
        static readonly string connectionString = @"Data Source=localhost; Database=simple_game; User ID=root; Password='Ghtjeo12'";

        /// <summary>
        /// 유저 정보 데이터를 가져온다.
        /// </summary>
        /// <param name="userName">가져올 유저의 이름</param>
        /// <returns>DB의 유저 스키마를 본 뜬 클래스 객체</returns>
        public User GetUser(string userName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "usp_get_user";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters["userName"].Direction = System.Data.ParameterDirection.Input;

                cmd.Parameters.Add("outUserName", MySqlDbType.VarChar);
                cmd.Parameters["outUserName"].Direction = System.Data.ParameterDirection.Output;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    if (userName.CompareTo(cmd.Parameters["outUserName"].Value as string) == 0)
                    {
                        User user = new User(cmd.Parameters["outUserName"].Value as string);
                        return user;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// DB에 유저가 존재하는지 체크한다.
        /// </summary>
        /// <param name="userName">존재 여부를 체크할 유저 이름</param>
        /// <returns>존재 여부</returns>
        public bool IsUserExists(string userName)
        {
            User user = GetUser(userName);
            if(user == null)
            {
                return false;
            }

            return true;
        }
    }
}