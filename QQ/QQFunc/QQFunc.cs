using MeowIOTBot.NetworkHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MeowIOTBot.NetworkHelper.PostHelper;

/// <summary>
/// 总操作函数库
/// </summary>
namespace MeowIOTBot.QQ.QQFunc
{
    /// <summary>
    /// QQ用户
    /// </summary>
    public class QQPerson
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int code;
        /// <summary>
        /// 存储区
        /// </summary>
        public datax data;
        /// <summary>
        /// 储存用的类
        /// </summary>
        public class datax
        {
            /// <summary>
            /// 头像地址
            /// </summary>
            public string avatarUrl;
            /// <summary>
            /// 头像bitmapHash
            /// </summary>
            public string bitmap;
            /// <summary>
            /// 保留字段
            /// </summary>
            public int commfrd;
            /// <summary>
            /// 朋友状态
            /// </summary>
            public int friendship;
            /// <summary>
            /// 所在地
            /// </summary>
            public string from;
            /// <summary>
            /// 性别
            /// </summary>
            public int gender;
            /// <summary>
            /// 活跃值
            /// </summary>
            public long intimacyScore;
            /// <summary>
            /// 是否为朋友
            /// </summary>
            public int isFriend;
            /// <summary>
            /// 
            /// </summary>
            public string logolabel;
            /// <summary>
            /// 昵称
            /// </summary>
            public string nickname;
            /// <summary>
            /// 腾讯会员等级状态
            /// </summary>
            public int qqvip;
            /// <summary>
            /// 是否开通QQ空间
            /// </summary>
            public int qzone;
            /// <summary>
            /// 真名
            /// </summary>
            public string realname;
            /// <summary>
            /// 其他名
            /// </summary>
            public string smartname;
            /// <summary>
            /// QQ号
            /// </summary>
            public long uin;
        }
        /// <summary>
        /// 默认行为
        /// </summary>
        public int @default;
        /// <summary>
        /// 返回的查询信息
        /// </summary>
        public string message;
        /// <summary>
        /// 子串码
        /// </summary>
        public int subcode;
        /// <summary>
        /// 获取实例的边界操作
        /// </summary>
        /// <param name="qqid">QQ号</param>
        /// <returns></returns>
        public async Task<QQPerson> GetPerson(long qqid) => JsonConvert.DeserializeObject<QQPerson>(await PASA(UrlType.GetUserInfo, $"{{\"UserID\":{ qqid}}}"));

    }
    /// <summary>
    /// QQ请求应答
    /// </summary>
    public class QQRequestResponse
    {
        /// <summary>
        /// 处理好友添加
        /// </summary>
        /// <param name="Action"> 1忽略 2同意 3拒绝 </param>
        public async Task<string> DealFriend(int Action) => await PASA(UrlType.DealFriend, $"{{\"Action\":{Action}}}");
        /// <summary>
        /// 处理群添加
        /// </summary>
        /// <param name="Action"> 11 同意 14 忽略 21 不同意 </param>
        public async Task<string> DealGroupInvite(int Action) => await PASA(UrlType.AnswerInviteGroup, $"{{\"Action\":{Action}}}");
    }
    /// <summary>
    /// 本账号操作
    /// </summary>
    public class QQMe
    {
        /// <summary>
        /// 获取在线账号的Cookie
        /// </summary>
        public async void GetUserCook() => await PASA(UrlType.GetUserCook, "");
    }
    /// <summary>
    /// 好友列表
    /// </summary>
    public class QQFriendList
    {
        /// <summary>
        /// QQ好友类
        /// </summary>
        public class QQFriend
        {
            /// <summary>
            /// 好友QQ号
            /// </summary>
            public long FriendUin;
            /// <summary>
            /// 是否含有备注
            /// </summary>
            public bool IsRemark;
            /// <summary>
            /// 好友昵称
            /// </summary>
            public string NickName;
            /// <summary>
            /// 在线字符串
            /// </summary>
            public string OnlineStr;
            /// <summary>
            /// 昵称
            /// </summary>
            public string Remark;
            /// <summary>
            /// 状态
            /// </summary>
            public int status;
        }
        /// <summary>
        /// 总好友数
        /// </summary>
        public int Friend_count;
        /// <summary>
        /// 被调用好友数
        /// </summary>
        public List<QQFriend> FriendList;
        /// <summary>
        /// 获取好友数
        /// </summary>
        public int GetfriendCount;
        /// <summary>
        /// 列表读取
        /// </summary>
        public int StartIndex;
        /// <summary>
        /// 好友总数
        /// </summary>
        public int Totoal_friend_count;
        /// <summary>
        /// 内部保存的SET集
        /// </summary>
        public class _FriendList
        {
            private List<QQFriend> q = new List<QQFriend>();
            /// <summary>
            /// 添加集方法
            /// </summary>
            /// <param name="n">项目</param>
            /// <returns></returns>
            public bool Add(QQFriend n)
            {
                if (q.Exists((QQFriend k) => k.FriendUin == n.FriendUin))
                {
                    return false;
                }
                else
                {
                    q.Add(n);
                    return true;
                }
            }
            /// <summary>
            /// 获取集
            /// </summary>
            /// <returns></returns>
            public List<QQFriend> getInstance() => this.q;
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<QQFriend>> GetFriendList()
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
        /// <summary>
        /// 获取列表的部分集合
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        private async Task<QQFriendList> GetListOnIndex(int Index) => JsonConvert.DeserializeObject<QQFriendList>(await PASA(UrlType.GetQQUserList, $"{{\"StartIndex\":{Index}}}"));
    }
    /// <summary>
    /// QQ好友请求(管理类)方法
    /// </summary>
    public class QQFriendRequest
    {
        /// <summary>
        /// 加好友总方法
        /// </summary>
        /// <param name="AddFromSource">2011 空间 2020 QQ搜索 2004 群组 2005 讨论组</param>
        /// <param name="FromGroupID">来源为2004时需要添加</param>
        /// <param name="Content">内容</param>
        /// <param name="AddUserUid">要添加的用户</param>
        /// <returns></returns>
        private async Task<string> __FriendManage(int AddFromSource, long FromGroupID, long AddUserUid, string Content) =>
                await PASA(
                    UrlType.AddQQUser,
                    $"{{\"AddUserUid\":{AddUserUid}," +
                    $"\"Content\":\"{Content}\"," +
                    $"\"AddFromSource\":{AddFromSource}," +
                    $"\"FromGroupID\":{FromGroupID}}}"
                );
        /// <summary>
        /// 从QQ搜索加人
        /// </summary>
        /// <param name="AddUserUid">要加的人QQ号</param>
        /// <param name="Content">验证内容</param>
        /// <returns></returns>
        public async Task<string> AddFriendFromQQSearch(long AddUserUid, string Content) => await __FriendManage(2020, 0, AddUserUid, Content);
        /// <summary>
        /// 从QQ空间加人
        /// </summary>
        /// <param name="AddUserUid">被加人的QQ</param>
        /// <param name="Content">验证内容</param>
        /// <returns></returns>
        public async Task<string> AddFriendFromQQSpace(long AddUserUid, string Content) => await __FriendManage(2011, 0, AddUserUid, Content);
        /// <summary>
        /// 从群内加人
        /// </summary>
        /// <param name="AddUserUid">被加人的QQ</param>
        /// <param name="fromGroupId">同在的群号</param>
        /// <param name="Content">验证内容</param>
        /// <returns></returns>
        public async Task<string> AddFriendFromQQGroup(long AddUserUid, long fromGroupId, string Content) => await __FriendManage(2004, fromGroupId, AddUserUid, Content);
        /// <summary>
        /// 从讨论组加人 (暂时弃用)
        /// </summary>
        /// <param name="AddUserUid">被加人的QQ</param>
        /// <param name="Content">验证内容</param>
        /// <returns></returns>
        [Obsolete]
        public async Task<string> AddFriendFromQQDisGroup(long AddUserUid, string Content) => await __FriendManage(2005, 0, AddUserUid, Content);
    }
    /// <summary>
    /// 群成员列表
    /// </summary>
    public class QQGroupUserList
    {
        /// <summary>
        /// 列对象
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 群号
        /// </summary>
        public long GroupUin { get; set; }
        /// <summary>
        /// 保留
        /// </summary>
        public int LastUin { get; set; }
        /// <summary>
        /// 群成员列表
        /// </summary>
        public List<QQGroupUser> MemberList { get; set; }
        /// <summary>
        /// QQ群成员
        /// </summary>
        public class QQGroupUser
        {
            /// <summary>
            /// 年龄
            /// </summary>
            public int Age;
            /// <summary>
            /// 未知属性
            /// </summary>
            public int FaceId;
            /// <summary>
            /// 性别
            /// </summary>
            public int Gender;
            /// <summary>
            /// 群昵称
            /// </summary>
            public string GroupCard;
            /// <summary>
            /// 加入群时间
            /// </summary>
            public long JoinGroupTime;
            /// <summary>
            /// 展示的群成员邮箱
            /// </summary>
            public string MemberEmail;
            /// <summary>
            /// 群成员QQ号
            /// </summary>
            public long MemberUin;
            /// <summary>
            /// 群成员昵称
            /// </summary>
            public string NickName;
            public int RankDes;
            /// <summary>
            /// 我的群成员备注
            /// </summary>
            public string Remark;
            /// <summary>
            /// 群成员最后一次发言时间
            /// </summary>
            public int SpeakTime;
            /// <summary>
            /// 群成员当前状态 
            /// 10在线 | 20隐身
            /// </summary>
            public int Status;
            /// <summary>
            /// 是否群管理员 0F/1T
            /// </summary>
            public int GroupAdmin;
        }
        /// <summary>
        /// 获取一个QQGroupUserList对象
        /// </summary>
        /// <param name="i">一个请求对象</param>
        /// <returns>返回一个标准的QQGroupUserList对象</returns>
        public async Task<List<QQGroupUser>> GetGroupUserList(long GroupId)
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
    }
    /// <summary>
    /// 群列表
    /// </summary>
    public class QQGroupList
    {
        /// <summary>
        /// 群总数
        /// </summary>
        public int Count;
        /// <summary>
        /// 拉取列表的下一个操作验证
        /// </summary>
        public int? NextToken;
        /// <summary>
        /// 拉取的部分群列表
        /// </summary>
        public List<QQGroup> TroopList;
        /// <summary>
        /// QQ群的操作列表
        /// </summary>
        public class QQGroup
        {
            /// <summary>
            /// 群号
            /// </summary>
            public long GroupId;
            /// <summary>
            /// 群成员数
            /// </summary>
            public int GroupMemberCount;
            /// <summary>
            /// 群名
            /// </summary>
            public string GroupName;
            /// <summary>
            /// 群公告(获取最近一条置顶)
            /// </summary>
            public string GroupNotice;
            /// <summary>
            /// 群主
            /// </summary>
            public long GroupOwner;
            /// <summary>
            /// 群总成员数
            /// </summary>
            public int GroupTotalCount;
        }
        /// <summary>
        /// 内套的群列表
        /// </summary>
        public class _GroupList
        {
            /// <summary>
            /// 私有的群列表
            /// </summary>
            private List<QQGroup> q = new List<QQGroup>();
            /// <summary>
            /// 重构的对比添加
            /// </summary>
            /// <param name="n">项</param>
            /// <returns></returns>
            public bool Add(QQGroup n)
            {
                if (q.Exists((QQGroup k) => k.GroupId == n.GroupId))
                {
                    return false;
                }
                else
                {
                    q.Add(n);
                    return true;
                }
            }
            /// <summary>
            /// 获取群列表
            /// </summary>
            /// <returns></returns>
            public List<QQGroup> getInstance() => this.q;
        }
        /// <summary>
        /// 获取所有群列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<QQGroup>> GetGroupList()
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
        /// 拉取群列表
        /// </summary>
        /// <param name="nt">索引号</param>
        /// <returns></returns>
        private async Task<QQGroupList> GetGroupListOnIndex(int nt) =>
            JsonConvert.DeserializeObject<QQGroupList>(
                await PASA(
                    UrlType.GetGroupList,
                    $"{{\"NextToken\"{nt}}}")
                );
    }
    /// <summary>
    /// QQ群请求(管理类)方法
    /// </summary>
    public class QQGroupRequest
    {
        /// <summary>
        /// 总方法
        /// </summary>
        /// <param name="ActionType">8拉人 1加入 2退出 3移出</param>
        /// <param name="GroupID">群号</param>
        /// <param name="ActionUserID">QQ用户ID *加入退出不用写* </param>
        /// <param name="Content">内容</param>
        private async Task<string> __GroupManage(int ActionType, long GroupID, long ActionUserID, string Content) => await
            PostHelper.PASA(
                PostHelper.UrlType.GroupMgr,
                $"{{\"ActionType\":{ActionType},\"GroupID\":{GroupID}," +
                $"\"ActionUserID\":{ActionUserID},\"Content\":\"{Content}\"}}"
                );
        /// <summary>
        /// 加群
        /// </summary>
        /// <param name="GroupId">加入的群群号</param>
        /// <param name="Content">验证内容</param>
        /// <returns></returns>
        public async Task<string> AddIntoGroup(long GroupId, string Content) => await __GroupManage(1, GroupId, 0, Content);
        /// <summary>
        /// 退群
        /// </summary>
        /// <param name="GroupId">群号</param>
        /// <param name="Content">内容</param>
        /// <returns></returns>
        public async Task<string> QuitGroup(long GroupId, string Content) => await __GroupManage(2, GroupId, 0, Content);
        /// <summary>
        /// 移除群员
        /// </summary>
        /// <param name="GroupId">群号</param>
        /// <param name="QQPerson">被移除者</param>
        /// <param name="Content">内容</param>
        /// <returns></returns>
        public async Task<string> RemoveSomeBodyFrom(long GroupId, long QQPerson, string Content) => await __GroupManage(3, GroupId, QQPerson, Content);
        /// <summary>
        /// 拉人到..
        /// </summary>
        /// <param name="GroupId">群号</param>
        /// <param name="QQPerson">被拉人</param>
        /// <param name="Content">内容</param>
        /// <returns></returns>
        public async Task<string> InviteSomeBodyInto(long GroupId, long QQPerson, string Content) => await __GroupManage(8, GroupId, QQPerson, Content);

    }
}
