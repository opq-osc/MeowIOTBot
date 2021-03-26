using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MeowIOTBot.QQ.QQFunc;
using static MeowIOTBot.NetworkHelper.PostHelper;
using static MeowIOTBot.QQ.QQFunc.QQGroupList;
using static MeowIOTBot.QQ.QQFunc.QQGroupUserList;
using static MeowIOTBot.QQ.QQFunc.QQGroupRequest;
using static MeowIOTBot.QQ.QQFunc.QQFriendList;
using static MeowIOTBot.QQ.QQFunc.QQFriendRequest;


namespace MeowIOTBot.QQ.QQAction
{
    /// <summary>
    /// 本账号操作
    /// <para>any parameter that in selfsponse</para>
    /// </summary>
    public class Me
    {
        /// <summary>
        /// 获取在线账号的Cookie
        /// <para>get the cookie of this Online(ed) QQ</para>
        /// </summary>
        public static async Task<QQCookie> GetUserCook() => JsonConvert.DeserializeObject<QQCookie>(await PASA(UrlType.GetUserCook, "{\"Flag\":false}"));
        /// <summary>
        /// 赞某个人
        /// <para>sent QQZan*</para>
        /// </summary>
        /// <param name="qid">
        /// 被赞人的QQ号
        /// <para>QQ number for somebosy recieve Zan Action</para>
        /// </param>
        public static async Task<string> ToZanSombody(long qid) => await PASA(UrlType.QQZan, $"{{\"UserID\":{qid}}}");
        /// <summary>
        /// 全局Log
        /// <para>RoundLog</para>
        /// </summary>
        /// <param name="consoleLev"> - </param>
        /// <param name="FileLevel"> - </param>
        /// <returns></returns>
        public static async Task<string> ServiceLog(int consoleLev = 8, int FileLevel = 3) => await PASA(UrlType.__Logs, $"{{\"ConsoleLevel\":{consoleLev},\"FileLevel\":{FileLevel}}}");
        /// <summary>
        /// (查询任意一个QQ用户)获取一个QQPerson实例的边界操作
        /// <para>is the operation that getPersondetailed Message</para>
        /// </summary>
        /// <param name="qqid">
        /// QQ号
        /// <para>Query QQ number</para>
        /// </param>
        /// <returns></returns>
        public static async Task<QQPerson> GetPerson(long qqid) => JsonConvert.DeserializeObject<QQPerson>(await PASA(UrlType.GetUserInfo, $"{{\"UserID\":{qqid}}}"));
    }
    /// <summary>
    /// QQ群操作
    /// <para>QQGroup Interve Operation</para>
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 设置群头衔(必须为群主)
        /// <para>Set GroupTitle(must As a GroupLeader)</para>
        /// </summary>
        /// <param name="GroupID">群号(QQGroupNumber)</param>
        /// <param name="UserID">用户QQ号(PersonQQNumber)</param>
        /// <param name="NewTitle">头衔名(最大六个字符)</param>
        public static async Task<string> __SetGUT(long GroupID,long UserID,string NewTitle) => await PASA(UrlType.SetUniqueTitle, $"{{\"GroupID\":{GroupID},\"UserID\":{UserID},\"NewTitle\":\"{NewTitle}\"}}");
        /// <summary>
        /// 撤回群内信息
        /// <para>Revoke Messsage from</para>
        /// </summary>
        /// <param name="GroupID">群号(GroupID)</param>
        /// <param name="MsgSeq">信息的序列号(SequenceNum)</param>
        /// <param name="MsgRandom">信息的校验码(RandomrizeParam)</param>
        /// <returns></returns>
        public static async Task<string> _RevokeGroupMessage(long GroupID,long MsgSeq,long MsgRandom) => await PASA(UrlType.RevokeMsg, $"{{\"GroupID\":{GroupID},\"MsgSeq\":{MsgSeq},\"MsgRandom\":{MsgRandom}}}");
        /// <summary>
        /// 禁言某个人员
        /// <para>shut Up somebody in group of...</para>
        /// </summary>
        /// <param name="GroupID">群号 (GroupId)</param>
        /// <param name="ShutUpUserID">被执行人QQ号 (execute user to..)</param>
        /// <param name="Minute">禁言时间[分钟] (Shutup for .. minute)</param>
        /// <returns></returns>
        public static async Task<string> _ShutUpSingle(long GroupID,long ShutUpUserID,int Minute) => await PASA(UrlType.ShutUpSingle, $"{{\"GroupID\":{GroupID},\"ShutUpUserID\":{ShutUpUserID},\"ShutTime\":{Minute}}}");
        /// <summary>
        /// 禁言全体群成员
        /// <para>Shutup for entire Group</para>
        /// </summary>
        /// <param name="GroupID">群号(GroupId)</param>
        /// <param name="ON">
        /// 是否开启禁言[关闭发送False]
        /// <para>Send True as ShutUP enable,vise versa</para>
        /// </param>
        /// <returns></returns>
        public static async Task<string> _ShutUpAll(long GroupID, bool ON)
        {
            int k = ON ? 1 : 0;
            return await PASA(UrlType.ShutUpEntirely, $"{{\"GroupID\":{GroupID},\"Switch\":{k}}}");
        }
        /// <summary>
        /// 更改群名片
        /// <para>change the GroupCard </para>
        /// </summary>
        /// <param name="qqgroup">群号<para>GroupNumber</para></param>
        /// <param name="userid">QQ号<para>QQNumber</para></param>
        /// <param name="newName">新的群名称</param>
        /// <returns></returns>
        public static async Task<string> _ModifyNickCard(long qqgroup, long userid, string newName) => await PASA(
            UrlType.ModifyGroupCard,
            $"{{\"GroupID\":{qqgroup},\"UserID\":{userid},\"NewNick\":\"{newName}\"}}");
        /// <summary>
        /// 获取一个QQGroupUserList对象
        /// <para>get a QQGroupList Object</para>
        /// </summary>
        /// <param name="GroupId">一个请求对象<para>The QQ Group Request Object</para></param>
        /// <returns>返回一个标准的QQGroupUserList对象<para>returns a standarded QQGroupList Object</para></returns>
        public static async Task<List<QQGroupUser>> GetGroupUserList(long GroupId)
        {
            long LastUin = 0;
            List<QQGroupUser> u = new List<QQGroupUser>();
            while (true)
            {
                var k = await PASA(
                    UrlType.GetGroupUserList,
                    $"{{\"GroupUin\":{GroupId},\"LastUin\":{LastUin}}}"
                    );
                var d = JsonConvert.DeserializeObject<QQGroupUserList>(k);
                u.AddRange(d.MemberList.ToArray());
                if (d.LastUin == 0)
                {
                    break;
                }
                else
                {
                    LastUin = d.LastUin;
                }
            }
            return u;
        }
        /// <summary>
        /// 获取所有群列表
        /// <para>get-all Group list</para>
        /// </summary>
        /// <returns></returns>
        public static async Task<List<QQGroup>> GetGroupList()
        {
            _GroupList u = new _GroupList();
            int index = 0;
            while (true)
            {
                var r = await GetGroupListOnIndex(index);
                r.TroopList.ForEach((k) => { u.Add(k); });
                if (r.NextToken == 0 || r.NextToken == null)
                {
                    break;
                }
            }
            return u.getInstance();
        }
        /// <summary>
        /// 加群<para>Joinin Group</para>
        /// <para>Addin Group</para>
        /// </summary>
        /// <param name="GroupId">加入的群群号<para>the Group Number</para></param>
        /// <param name="Content">验证内容<para>the identify content</para></param>
        /// <returns></returns>
        public static async Task<string> AddIntoGroup(long GroupId, string Content) => await __GroupManage(1, GroupId, 0, Content);
        /// <summary>
        /// 退群<para>Leave Group</para>
        /// </summary>
        /// <param name="GroupId">加入的群群号<para>the Group Number</para></param>
        /// <param name="Content">验证内容<para>the identify content</para></param>
        /// <returns></returns>
        public static async Task<string> QuitGroup(long GroupId, string Content) => await __GroupManage(2, GroupId, 0, Content);
        /// <summary>
        /// 移除群员<para>remove Group Member</para>
        /// </summary>
        /// <param name="GroupId">加入的群群号<para>the Group Number</para></param>
        /// <param name="Content">验证内容<para>the identify content</para></param>
        /// <param name="QQPerson">被移除者<para>the one that will be execute</para></param>
        /// <returns></returns>
        public static async Task<string> RemoveSomeBodyFrom(long GroupId, long QQPerson, string Content) => await __GroupManage(3, GroupId, QQPerson, Content);
        /// <summary>
        /// 拉人到..
        /// <para>inviteSomeone</para>
        /// </summary>
        /// <param name="GroupId">加入的群群号
        /// <para>the Group Number</para>
        /// </param>
        /// <param name="Content">验证内容
        /// <para>the identify content</para>
        /// </param>
        /// <param name="QQPerson">被拉人
        /// <para>the one that you ingage in</para>
        /// </param>
        /// <returns></returns>
        public static async Task<string> InviteSomeBodyInto(long GroupId, long QQPerson, string Content) => await __GroupManage(8, GroupId, QQPerson, Content);
    }
    /// <summary>
    /// QQ好友相关操作
    /// <para>QQFriend Interve Opeation</para>
    /// </summary>
    public class Friend
    {
        /// <summary>
        /// 从QQ搜索加人
        /// <para>Add Friend From QQSearch</para>
        /// </summary>
        /// <param name="AddUserUid">要加的人QQ号<para>the QQnumber that you want to become friend with</para></param>
        /// <param name="Content">验证内容<para>the specific content you want to send</para></param>
        /// <returns></returns>
        public static async Task<string> AddFriendFromQQSearch(long AddUserUid, string Content) => await __FriendManage(2020, 0, AddUserUid, Content);
        /// <summary>
        /// 从QQ空间加人
        /// </summary>
        /// <param name="AddUserUid">要加的人QQ号<para>the QQnumber that you want to become friend with</para></param>
        /// <param name="Content">验证内容<para>the specific content you want to send</para></param>
        /// <returns></returns>
        public static async Task<string> AddFriendFromQQSpace(long AddUserUid, string Content) => await __FriendManage(2011, 0, AddUserUid, Content);
        /// <summary>
        /// 从群内加人
        /// </summary>
        /// <param name="AddUserUid">要加的人QQ号<para>the QQnumber that you want to become friend with</para></param>
        /// <param name="Content">验证内容<para>the specific content you want to send</para></param>
        /// <param name="fromGroupId">同在的群号<para>the specific group that you two both in</para></param>
        /// <returns></returns>
        public static async Task<string> AddFriendFromQQGroup(long AddUserUid, long fromGroupId, string Content) => await __FriendManage(2004, fromGroupId, AddUserUid, Content);
        /// <summary>
        /// 获取好友列表
        /// <para>get the QQFriend List</para>
        /// </summary>
        /// <returns></returns>
        public static async Task<List<QQFriend>> GetFriendList()
        {
            _FriendList u = new _FriendList();
            int index = 0;
            while (true)
            {
                var r = await GetListOnIndex(index);
                r.FriendList.ForEach((k) => { u.Add(k); });
                if (r.Totoal_friend_count > index)
                {
                    index = ((index + r.GetfriendCount) > r.Totoal_friend_count ? r.Totoal_friend_count : index += r.GetfriendCount);
                }
                else
                {
                    break;
                }
            }
            return u.getInstance();

        }
    }
}
